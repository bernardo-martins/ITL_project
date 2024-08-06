import { createApp, ref } from 'vue'
import App from './App.vue'
import router from './router'
import Primevue from 'primevue/config'
import Menubar from 'primevue/menubar'
import InputText from 'primevue/inputtext'
import Badge from 'primevue/badge'
import Checkbox from 'primevue/checkbox'
import Button from 'primevue/button'
import Card from 'primevue/card'
import Sidebar from 'primevue/sidebar'
import Ripple from 'primevue/ripple'
import 'primevue/resources/themes/luna-pink/theme.css'
import "primeflex/primeflex.css";
import Avatar from 'primevue/avatar'
import StyleClass from 'primevue/styleclass';
import 'primeicons/primeicons.css'
import store from './store'
import axios from 'axios'
import FullCalendar from '@fullcalendar/vue3'
import dayGridPlugin from '@fullcalendar/daygrid'
import Chart from 'primevue/chart'
import Message from 'primevue/message'
import ToastService from 'primevue/toastservice'
import Toast from 'primevue/toast'

const app = createApp(App)


app.use(router)
app.use(Primevue)
app.use(store)
app.component('Menubar', Menubar) // Make sure the name matches what you use in the template
app.component('InputText', InputText)
app.component('Badge', Badge)
app.component('Checkbox', Checkbox)
app.component('Button', Button)
app.component('Card', Card)
app.component('Sidebar', Sidebar)
app.component('Avatar', Avatar)
app.component('FullCalendar', FullCalendar)
app.component('Chart', Chart)
app.component('Message', Message)
app.component('Toast', Toast)
app.component('ToastService', ToastService)
app.component('dayGridPlugin', dayGridPlugin)
app.directive('ripple', Ripple)
app.directive('styleclass', StyleClass)




axios.defaults.baseURL = process.env.NODE_ENV == 'production' ? "/api" : "http://localhost:5169/api";
app.mount('#app')
