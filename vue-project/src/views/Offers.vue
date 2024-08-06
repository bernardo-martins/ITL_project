<script>
import 'primevue/resources/themes/luna-pink/theme.css'
import "primeflex/primeflex.css";
import 'primeicons/primeicons.css';
import Card from "primevue/card";
import axios from 'axios';
import Button from 'primevue/button';


export default {
    name: 'Offers',
    data: function () 
    {
      return {
        
        
        courses: [
          {
            image: 'https://http.cat/images/202.jpg',
            title: 'Katzen Tierhaltung Kurs 1',
            subtitle: 'subtitle',
            description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Inventore sed consequuntur error repudiandae numquam deserunt quisquam repellat libero asperiores earum nam nobis, culpa ratione quam perferendis esse, cupiditate neque quas!'
          },
          {
            image: 'https://http.cat/images/203.jpg',
            title: 'Katzen Tierhaltung Kurs 2',
            subtitle: 'subtitle',
            description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Inventore sed consequuntur error repudiandae numquam deserunt quisquam repellat libero asperiores earum nam nobis, culpa ratione quam perferendis esse, cupiditate neque quas!'
          }
        ],

        components: {
        Card
        },
      
      }
    ////
      addtoCart()
      {
        console.log("addtocart");
      }

      mounted()
      {
        // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
        // automatisch vorangestellt
        const courseId = this.addToCart();
        const response =  axios.get(`course/${courseId}`);
        this.courses = response.data;
        console.log(this.courses);
      }

    }
  }
  /* methods: {
    addToCart(courseId) 
    {
      console.log("addToCart", courseId);
      axios.post('http://localhost:3000/cart', { courseId })
        .then(() => {
          swal.fire({
            title: 'Kurs hinzugefügt',
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
          });
        })
        .catch(() => {
          swal.fire({
            title: 'Fehler beim Hinzufügen',
            icon: 'error',
            showConfirmButton: false,
            timer: 1500
          });
        });
    }
  } */
</script>

<template>
    <div class="xoffer-container">
        <div class="grid">
          <div v-for="course in courses" class="col-12 md:col-6 lg:col-4">
            <Card class="offer-card">
              <template #header>
                <div class="course-image-wrapper">
                  <img class="course-image" alt="user header" :src="course.image" />
                </div>

              </template>
              <template #title v-if="course.title">{{ course.title }}</template>
              <template #subtitle v-if="course.subtitle">{{ course.subtitle }}e</template>
              <template #content>
                <p class="m-0" v-if="course.description">
                  {{ course.description }}
                </p>
              </template>
              <template #footer>
                <div class="flex gap-3 mt-1">
                  <Button label="Mehr zu diesem Kurs" severity="secondary" outlined class="w-full" />
                  <Button id="add-to-cart-button" label="Kaufen" class="w-full" @click="addToCart" />
                </div>
              </template>
            </Card>
          </div>
        </div>
    </div>
</template>

<style scoped>

.offer-card {
  margin-bottom: 20px;
}

.course-image{
  width: 100%;
  height: auto;
  object-fit: contain;
}

.course-image-wrapper{
  height: 320px;
  overflow: hidden;
}
</style>