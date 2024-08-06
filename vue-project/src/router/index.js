import { createRouter, createWebHistory } from 'vue-router'
import LoginPage from '../views/LoginPage.vue'
import CoursePage from '../views/CoursePage.vue'
import Offers from '../views/Offers.vue'
import UserCourses from '../views/UserCourses.vue'
import MyAccount from '../views/MyAccount.vue'
import Cart from '../views/Cart.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
    },
    {
      path: '/LoginPage',
      name: 'LoginPage',
      component: LoginPage
    },
    {
      path: '/:courseGuid/:lectionGuid',
      name: 'CoursePage',
      component: CoursePage,
      props: true
    },
    {
      path: '/Offers',
      name: 'Offers',
      component: Offers
    },
    {
      path: '/UserCourses',
      name: 'UserCourses',
      component: UserCourses
    },
    {
      path: '/MyAccount/:userGuid',
      name: 'MyAccount',
      component: MyAccount,
      props: true
    },
    {
      path : '/Cart',
      name: 'Cart',
      component: Cart
    }
  ]
})

export default router
