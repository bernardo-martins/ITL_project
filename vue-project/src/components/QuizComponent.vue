<template>
  <div>
    <div v-if="filteredQuizzes.length === 0">
      <Card style="background-color: rgba(0, 0, 0, 0.4); width: 80%; border-radius: 30px; border: 2px solid #F48FB1;">
        <template #content>
          <h1>Keine weiteren Quizze verfügbar</h1>
        </template>
      </Card>
    </div>
    <div v-else>
      <div v-for="(quiz, quizIndex) in filteredQuizzes" :key="quiz.quizGuid">
        <h1>{{ quiz.title }}</h1>
        <div v-for="(question, questionIndex) in quiz.questions" :key="question.questionGuid">
          <Card style="background-color: rgba(0, 0, 0, 0.4); width: 80%; border-radius: 30px; border: 2px solid #F48FB1;">
            <template #content>
              <h2>{{ question.questionText }}? </h2>
              <div v-for="(answer, answerIndex) in question.answers" :key="answerIndex">
                <Button :label="answer" :class="{
                  'button-quiz': true,
                  'button-quiz-selected': selectedAnswers[quizIndex]?.[question.questionGuid] === answer,
                  'button-quiz-correct': correctAnswers[quiz.quizGuid]?.[question.questionGuid] === true && selectedAnswers[quizIndex]?.[question.questionGuid] === answer,
                  'button-quiz-incorrect': correctAnswers[quiz.quizGuid]?.[question.questionGuid] === false && selectedAnswers[quizIndex]?.[question.questionGuid] === answer
                }" @click="selectAnswer(quizIndex, question.questionGuid, answer)" />
              </div>
            </template>
          </Card>
        </div>
        <div>
          <h2 v-if="scores[quiz.quizGuid] !== undefined">Erreichte Punktzahl: {{ scores[quiz.quizGuid] }} / {{ quiz.questions.length }}</h2>
        </div>
        <button @click="submitQuiz(quizIndex)" class="button-submit">Submit</button>
      </div>
    </div>
  </div>
</template>

<script>
import Card from 'primevue/card';
import Button from 'primevue/button';
import axios from 'axios';

export default {
  components: {
    Card,
    Button
  },
  props: {
    lectionQuiz: {
      type: Array,
      required: true
    },
    userGuid: {
      type: [String, Number],
      required: true
    }
  },
  data() {
    return {
      selectedAnswers: {},
      correctAnswers: {},
      correctAnswersDetails: {},
      completedQuizzes: [], // Quizzes die schon completed sind vom User
      recentlyCompletedQuizzes: [], // Quizzes die gerade completed wurden während User auf der Seite ist 
      scores: {}
    };
  },
  computed: {
    filteredQuizzes() {
      return this.lectionQuiz.filter(
      quiz => 
        !this.completedQuizzes.includes(quiz.quizGuid.toString()) ||
        this.recentlyCompletedQuizzes.includes(quiz.quizGuid.toString())
    );}
  },
  watch: {
    lectionQuiz: {
      handler(newVal) {
        this.checkQuizzes();
      },
      deep: true,
      immediate: true
    },
    $route(to, from) {
    if (to.path !== from.path) {
      // Der Benutzer hat die Lektion gewechselt oder kehrt zurück, `recentlyCompletedQuizzes` leeren
      this.recentlyCompletedQuizzes = [];
    }
  }
  },
  methods: {
    selectAnswer(quizIndex, questionGuid, answer) {
      this.selectedAnswers = {
        ...this.selectedAnswers,
        [quizIndex]: {
          ...this.selectedAnswers[quizIndex],
          [questionGuid]: answer
        }
      };
    },
    async submitQuiz(quizIndex) {
      const quiz = this.filteredQuizzes[quizIndex]; // muss filteredQuizzes sein, sonst nimmt er alle quizzes und den falschen Index
      const quizGuid = quiz.quizGuid;
      const newAnswers = this.selectedAnswers[quizIndex];

      if (!newAnswers || Object.keys(newAnswers).length === 0) {
        console.error('No answers selected for quiz index:', quizIndex);
        return;
      }

      const answerDictionary = Object.fromEntries(
        Object.entries(newAnswers).map(([questionGuid, answer]) => [questionGuid, answer])
      );
      console.log('Quiz Guid:', quizGuid);
      console.log('Answers:', answerDictionary);

      try {
        const response = await axios.post(`/quizzes/check`, {
          quizGuid: quizGuid,
          userAnswers: answerDictionary
        }, {
          headers: {
            'Content-Type': 'application/json'
          }
        });

        const correctAnswersForQuiz = quiz.questions.reduce((acc, question, index) => {
          acc[question.questionGuid] = response.data.success[index];
          return acc;
        }, {});

        const correctAnswersDetailsForQuiz = quiz.questions.reduce((acc, question) => {
          const correctOption = question.answers.find(answer => correctAnswersForQuiz[question.questionGuid] && answer === this.selectedAnswers[quizIndex][question.questionGuid]);
          acc[question.questionGuid] = correctOption || 'Keine richtige Antwort ausgewählt';
          return acc;
        }, {});

        const score = response.data.success.filter(isCorrect => isCorrect).length;
        console.log('Correct answers:', correctAnswersForQuiz);
        console.log('Score:', score);

        this.correctAnswers = {
          ...this.correctAnswers,
          [quizGuid]: correctAnswersForQuiz
        };

        this.correctAnswersDetails = {
          ...this.correctAnswersDetails,
          [quizGuid]: correctAnswersDetailsForQuiz
        };

        this.scores = {
          ...this.scores,
          [quizGuid]: score
        };

        // Das Quiz wurde gerade completed
        this.recentlyCompletedQuizzes.push(quizGuid.toString());
        console.log('Recently completed quizzes:', this.recentlyCompletedQuizzes);
        console.log('Filtered Quizzes:', this.filteredQuizzes);

        // Call completeQuiz after successfully submitting the quiz
        await this.completeQuiz(quizGuid);

      } catch (error) {
        console.error('Error:', error);
      }
    },
    async checkQuizzes() {
      try {
        const response = await axios.get(`/UserQuizzes/get-userquiz/${this.userGuid}`);
        this.completedQuizzes = response.data.map(quiz => quiz.quizGuid.toString());
        // Clear recently completed quizzes since we are reloading
        
        console.log('Completed quizzes:', this.completedQuizzes);
      } catch (error) {
        console.error('Error fetching user quizzes:', error);
      }
    },
    async completeQuiz(currentquizGuid) {
      try {
        const response = await axios.post('/UserQuizzes/add-userquiz', {
          userGuid: this.userGuid,
          quizGuid: currentquizGuid
        }, {
          headers: {
            'Content-Type': 'application/json'
          }
        });
        console.log("Quiz completed", response.data);
        // Re-check quizzes after completing one
        await this.checkQuizzes();
      } catch (error) {
        console.error('Error completing quiz:', error);
      }
    }
  }
};
</script>

<style>
.button-quiz {
  background-color: #323232;
  margin-left: 15px;
  width: 80%;
  border-radius: 0px;
  color: white;
  font-size: 20px;
  text-align: left;
  margin-bottom: 10px;
}

.button-quiz-selected {
  background-color: #F48FB1 !important;
  color: white;
}

.button-quiz-correct {
  background-color: #4CAF50 !important;
  color: white !important;
}

.button-quiz-incorrect {
  background-color: #F44336 !important;
  color: white !important;
}

.button-submit {
  background-color: #F48FB1;
  width: 40%;
  height: 50px;
  margin-left: 20px;
  border-radius: 30px;
  border: 2px solid white;
}
</style>
