<script setup>
import { ref } from 'vue';
import MenubarAnika from '@/components/MenuBarAnika.vue';
import FooterAnika from '@/components/FooterAnika.vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const isCoursePage = ref(false);

router.afterEach((to) => {
  isCoursePage.value = to.name === 'CoursePage';
});

const handleSidebarToggle = () => {
  if (isCoursePage.value) {
    const coursePageComponent = router.currentRoute.value.matched[0].instances.default;
    if (coursePageComponent) {
      coursePageComponent.toggleSidebar();
    }
  }
}
</script>

<template>
  <div id="app">
    <MenubarAnika @toggle-sidebar="handleSidebarToggle" />
    <router-view />
    <FooterAnika />
  </div>
</template>
   

<style>
html, body {
  height: 100%; 
  margin: 0; 
  padding: 0; 
}
body {
  display: flex;
  flex-direction: column; 
}
#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

#app > .content {
  flex: 1;
}
</style>
