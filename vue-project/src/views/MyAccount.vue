<script>
import 'primevue/resources/themes/luna-pink/theme.css'
import "primeflex/primeflex.css";
import 'primeicons/primeicons.css';
import Card from "primevue/card";
import Button from "primevue/button";
import axios from 'axios';
import { mapGetters } from 'vuex';
import FullCalendar from '@fullcalendar/vue3'
import dayGridPlugin from '@fullcalendar/daygrid'
import Chart from 'primevue/chart';


export default {
    name: 'MyAccount',
    data: function () {
        return {
            usersbe: [], // UserDaten

            userscompletedlast: [], //Lektionen die der User zuletzt gelernt hat

            usercompleted: [], //Alle Lektionen die der User gelernt hat

            calendarOptions: {
                plugins: [dayGridPlugin],
                initialView: 'dayGridMonth',
                weekends: true,
                expandRows: true,
                events: [],
                eventContent: function (arg) {
                    let iconEl = document.createElement('i');
                    iconEl.className = arg.event.extendedProps.icon;
                    let titleEl = document.createElement('div');
                    titleEl.innerHTML = arg.event.title;
                    return { domNodes: [iconEl, titleEl] }
                },
                height: 'auto',
                contentHeight: 'auto'
            },
            chartData: null,
            chartOptions: null
        }
    },
    components: {
        Card,
        Button,
        FullCalendar,
        Chart
    },
    async mounted() {
        // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
        // automatisch vorangestellt
        
        const response = await axios.get(`users/user/${this.$route.params.userGuid}`); //gibt mir die UserDaten
        const response2 = await axios.get(`UserLectionCompletions/last/${this.$route.params.userGuid}`); //gibt mir die Daten der letzten 3 Lektionen
        const response3 = await axios.get(`UserLectionCompletions/${this.$route.params.userGuid}`);
        this.usersbe = response.data;
        this.usercompletedlast = response2.data;
        this.usercompleted = response3.data;
        this.chartData = this.setChartData();
        this.chartOptions = this.setChartOptions();
        this.updateCalendarEvents();
    },
    computed: {
        ...mapGetters({
            isAuthenticated: 'isAuthenticated',  // Assuming the getter's name in store is 'isAuthenticated'
            getUser: "getUser"
        }),
        userName() {
            return this.getUser.email
        },
        userGuid() {
            return this.getUser.guid
        }
    },
    methods: {
        setChartData() {
            return {
                labels: ['Clickertraining', 'Verhaltenstraining', 'Klo-Management'],
                datasets: [
                    {
                        data: [300, 50, 100],
                        backgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ],
                        hoverBackgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ]
                    }
                ]
            };
        },
        setChartOptions() {
            return {
                responsive: true,
                maintainAspectRatio: false
            };
        },
        updateCalendarEvents() {
            const groupedByDate = this.usercompleted.reduce((acc, completion) => {
                const date = new Date(completion.completionDate).toDateString();
                if (!acc[date]) {
                    acc[date] = [];
                }
                acc[date].push(completion);
                return acc;
            }, {});

            // Create events, limiting to 3 per day
            const events = Object.keys(groupedByDate).flatMap(date => {
                return groupedByDate[date].slice(0, 3).map(completion => ({
                    start: new Date(completion.completionDate),
                    extendedProps: { icon: 'pi pi-book' }
                }));
            });

        this.calendarOptions.events = events; // Update calendar events
        }
    
    }
}
</script>

<template>

    <!-- <div> {{ isAuthenticated }}</div>
    <div>{{ userName }} hi</div>
    <div class="offer-container">
        <div v-for="beuser in usersbe">
            <div v-if="beuser.username == user"> Hello {{ user }} </div>
        </div>
    </div> -->

    <div class="offer-container">
        <Card style="width: 33rem; height: 40rem; overflow:hidden" class="offer-container offer-card">
            <template #title>{{ userName }}</template>
            <template #content>
                <div class="grid-container">
                    <div class="content-left m-0">
                        <img src="/images/Balthi.png" alt="user header" class="profilepic" />
                        <p>Vorname: Niklas</p>
                        <p>Nachname: Moritz</p>
                        <p>Katze: Cassy</p>
                    </div>
                    <div class="content-right"></div>
                </div>
            </template>
        </Card>
        <Card style="width: 55rem; height: 40rem; overflow:hidden" class="offer-container offer-card">
            <template #content>
                <div class="calendar-container">
                    <FullCalendar :options='calendarOptions' />
                </div>
            </template>
        </Card>
        <Card style="width: 100%; height: auto; overflow:hidden" class="offer-container offer-card last-card">
            <template #content>
                <div class="grid-container grid-container-last">
                    <div class="content-left content-left-last">
                        <h1>Clickertraining</h1>
                        <p>Zuletzt gelernt</p>
                        <ul>
                            <li v-for="completion in usercompletedlast" :key="usercompletedlast.userID"> <!-- Zeig die letzten 3 gelernten Lektionen nach Datum an  -->
                                {{ completion.title }}
                            </li>
                        </ul>
                        <p>Nächse Lektion</p>
                        <ul>
                            <li>Die richtige Belohnung</li>
                        </ul>
                    </div>
                    <div class="content-right">
                        <h1 class="chart-heading">Verbrachte Zeit im Kurs</h1>
                        <div class="card flex justify-content-center">
                            <Chart type="pie" :data="chartData" :options="chartOptions" style="width: 250px; height: 250px;"/>
                        </div>
                    </div>
                </div>
            </template>
        </Card>
    </div>
</template>

<style scoped>
.offer-container {
    display: flex;
    flex-wrap: wrap;
    margin-top: 10px;
}

.offer-card {
    margin-bottom: 20px;
    margin-left: 20px;
}

.last-card {
    margin-right: 20px;
}

.grid-container {
    display: flex;
    justify-content: space-between;
}

.grid-container-last {
    margin-top: -50px;
}

.content-right,
.content-left {
    flex: 1;
    padding: 10px;
}

.content-left {
    flex: 1;
    border-right: 2px;
}

.content-left-last{
    margin-right: 250px;
}

.profilepic {
    height: 300px;
}

.calendar-container {
    height: calc(100% - 20px);
    /* Adjust height to make room for bottom margin */
    width: 100%;
    margin-bottom: 20px;
    /* Add margin at the bottom */
    overflow: hidden;
    box-sizing: border-box;
}

.fc {
    width: 100%;
    max-width: 100%;
    height: 100%;
}

.chart-heading {
    text-align: center;
    white-space: nowrap; /* Prevents line break */
    width: 100%;
}
</style>