<script>
import 'primevue/resources/themes/luna-pink/theme.css'
import "primeflex/primeflex.css";
import 'primeicons/primeicons.css';
import Card from "primevue/card";
import Button from "primevue/button";
import axios from 'axios';
import { mapGetters } from 'vuex';

export default {
    name: 'UserCourses',
    data: function () {
        return {
            courses: []
        }
    },
    components: {
        Card,
        Button
    },
    async mounted() {
        // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
        // automatisch vorangestellt
        const userGuid = this.getUserGuid();
        const response = await axios.get('users/${userGuid}/courses');
        this.courses = response.data;
        console.log(this.courses);
    },
    computed: {
        ...mapGetters({
            isAuthenticated: 'isAuthenticated',  
            getUser : "getUser"
        })
    },
    methods: {
        getUserGuid() {
            return this.getUser.guid;
        },
        async startCourse(course) {
        try {
        const response = await axios.get('lections/${course.courseGuid}');
        if (response.data.length > 0) {
            const firstLectionGuid = response.data[0].lectionGuid;
            this.$router.push({ name: 'CoursePage', params: { courseGuid: course.courseGuid, lectionGuid: firstLectionGuid }});
        } else {
            console.error('No lections available for this course');
        }
    } catch (error) {
        console.error('Failed to fetch lections:', error);
    }
}
    }
  }
</script>

<template>
    <div class="offer-container">
        <Card v-for="course in courses" :key="course.courseGuid" style="width: calc(33.33% - 10px); overflow: hidden" class="offer-card">
        <template #header>
            <img alt="user header" src="/public/images/Balthi.png" />
        </template>
            <template #title>{{ course.courseDescription }}</template>
        <template #subtitle>Inhalt</template>
        <template #content>
            <p class="m-0">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit. Inventore sed consequuntur error repudiandae numquam deserunt quisquam repellat libero asperiores earum nam nobis, culpa ratione quam perferendis esse, cupiditate neque
                quas!
            </p>
        </template>
        <template #footer>
            <div class="footer-buttons">
                <Button label="Starten" class="w-full" @click="startCourse(course)"/>
            </div>
        </template>
        </Card>
    </div>
</template>

<style scoped>
.offer-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: flex-start;
  gap: 10px;
  margin-top: 20px;
}

.offer-card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  width: calc(33.33% - 10px);
  margin-bottom: 20px;
  overflow: hidden;
  min-height: 300px; 
}

.footer-buttons {
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-bottom: 10px;
}
</style>
