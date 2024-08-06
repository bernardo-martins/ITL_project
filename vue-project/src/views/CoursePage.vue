<!-- File: src/views/Coursepage.vue -->
<script>
import 'primeicons/primeicons.css';
import 'primevue/resources/themes/luna-pink/theme.css';
import "primeflex/primeflex.css";
import SideBarCourses from '@/components/SideBarCourses.vue';
import Card from 'primevue/card';
import axios from 'axios';
import HlsPlayer from '@/components/HlsPlayer.vue';
import Button from 'primevue/button';
import Message from 'primevue/message';
import { mapGetters } from 'vuex';
import QuizComponent from '@/components/QuizComponent.vue';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';

export default {
  components: {
    SideBarCourses,
    HlsPlayer,
    Button,
    Card,
    Message,
    QuizComponent,
    Toast
  },
  data() {
    return {
      usersbe: [],
      sidebarVisible: false,
      lectionschild: [],
      currentLection: {},
      lectionQuiz: [],
      videoUrl: "",
      tokenUrl: "",
      isCompleted: false,
      isVideoCompleted: false,
      showMessage: false
    };
  },
  props: {
    courseGuid: {
      type: String,
      required: true
    },
    lectionGuid: {
      type: String,
      required: false
    }
  },
  computed: {
    ...mapGetters({
      isAuthenticated: 'isAuthenticated',
      getUser: "getUser"
    }),
    userName() {
      return this.getUser.email;
    },
    userGuid() {
      return this.getUser.guid;
    }
  },
  async mounted() {
    await this.fetchCourseDetails();
    if (this.$route.params.lectionGuid) {
      await this.fetchLectionDetails(this.$route.params.lectionGuid);
    }
    const response = await axios.get(`users/user/${this.userGuid}`);
    this.usersbe = response.data;
    console.log(this.userGuid);
    console.log(this.userName);
  },
  watch: {
    '$route.params.lectionGuid': 'fetchLectionDetails'
  },
  methods: {
    handleVideoEnd() {
      this.isVideoCompleted = true;
    },
    async fetchCourseDetails() {
      const courseGuid = this.courseGuid;
      const response = await axios.get(`/lections/${courseGuid}`);
      this.lectionschild = response.data;
    },
    async checkIfCompleted(lectionGuid) {
      try {
        const response = await axios.get(`UserLectionCompletions/completed/${this.userGuid}/${lectionGuid}`);
        this.isCompleted = response.data;
      } catch (error) {
        console.error("Error checking if lection is completed:", error);
        return false;
      }
    },
    async fetchLectionDetails(lectionGuid) {
      if (!lectionGuid) return;
      try {
        const response = await axios.get(`/lections/lection/${lectionGuid}`);
        const response2 = await axios.get(`Quizzes/${lectionGuid}`);
        this.currentLection = response.data;
        this.lectionQuiz = response2.data;
        await this.fetchVideoUrl();
        this.checkIfCompleted(lectionGuid);
      } catch (error) {
        console.error("Failed to fetch lection details:", error);
      }
    },
    async fetchVideoUrl() {
      try {
        const blobName = 'master.m3u8';
        const response = await axios.get(`/blobs/get-sas-token/anikatzevideo01`);
        const data = response.data.sasTokenUrl;
        const mytest = "anikatzevideo01/master.m3u8";
        const baseUrl = "https://anikatzecdn.azureedge.net/";
        this.tokenUrl = data.split('anikatzevideo01')[1];
        this.videoUrl = baseUrl + mytest;
      } catch (error) {
        console.error('Error fetching video URL:', error);
      }
    },
    handleVisibilityChange(newVisibility) {
      this.sidebarVisible = newVisibility;
    },
    toggleSidebar() {
      this.sidebarVisible = !this.sidebarVisible;
    },
    async completelection() {
      try{
      const response = await axios.post('/UserLectionCompletions/add-completion', {
        LectionGuid: this.currentLection.lectionGuid,
        timeSpent: "00:20:00",
        UserGuid: this.userGuid
      });
      this.isCompleted = true;
      this.showMessage = true;
      } catch (error) {
        console.error('Error completing lection:', error);
      }
    }
  }
};
</script>

<template>
  <div class="full-screen">
    <SideBarCourses v-show="sidebarVisible" :visible="sidebarVisible" @update:visible="handleVisibilityChange" :lections="lectionschild" />
    <Card>
      <template #title>{{ currentLection.title }}</template>
      <template #content>
        <div class="video-container">
          <HlsPlayer :src="videoUrl" :sasToken="tokenUrl" @ended="handleVideoEnd" />
        </div>
      </template>
    </Card>
    <Card>
      <template #title>{{ currentLection.title }}</template>
      <template #content>
        <p>{{ currentLection.text }}</p>
      </template>
    </Card>
    <div v-if="isVideoCompleted">
      <QuizComponent :lectionQuiz="lectionQuiz" :userGuid="userGuid" />
    </div>
    <div v-else>
      <Card>
        <template #content>
          <h1>Quiz ist verfügbar sobald das Video zu Ende ist!</h1>
        </template>
      </Card>
    </div>
    <Card style="background-color: rgba(0, 0, 0, 0.4); width: 80%; border-radius: 30px; border: 2px solid #F48FB1;">
      <!--  <template #title>Lektion abgeschlossen?</template> -->
      <template #content>
        <div v-if="!isCompleted" class="center-container">
          <Button label="Lektion abschließen" @click="completelection()" style="border-radius: 30px; font-size: large;" />
        </div>
        <div v-else class="card flex justify-content-center">
          <h1>Lektion abgeschlossen!</h1>
        </div>
        <div v-if="showMessage">
          <Message severity="success"> Du hast die Lektion nun abgeschlossen! </Message>
        </div>
        <!-- <div>
          <p>{{ userGuid }}</p>
          <p>{{ currentLection.lectionGuid }}</p>
        </div> -->
      </template>
    </Card>
  </div>
</template>

<style>
.full-screen {
  display: flex;
  flex-direction: column;
}

.full-screen .p-card {
  flex: 1;
  margin: 20px;
}

.video-container {
  display: flex;
  justify-content: center;
  align-items: center;
}

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
.center-container {
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>