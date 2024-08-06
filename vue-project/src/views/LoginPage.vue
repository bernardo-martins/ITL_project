<script>
import InputText from 'primevue/inputtext';
import Checkbox from 'primevue/checkbox';
import Button from 'primevue/button';
import { mapActions } from 'vuex';
import axios from 'axios';

export default {
    name: 'LoginPage',
    components: {
        InputText,
        Checkbox,
        Button
    },
    data() {
        return {
            guid: '', // Change id to guid
            email: '',
            password: '',
            rememberMe: false,
            foundUser: null,
            checked1: false,
            usersbe: [] // Add usersbe to data
        };
    },
    async mounted() {
        // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
        // automatisch vorangestellt
        const response = await axios.get('users');
        this.usersbe = response.data;
    },
    methods: {
        ...mapActions(['login']),
        async performLogin() {
            const userCredentials = { email: this.email, password: this.password, rememberMe: this.rememberMe };
            let foundUser = null;
            for (let i = 0; i < this.usersbe.length; i++) {
                if (this.usersbe[i].username === this.email) {
                    foundUser = { 
                        guid: this.usersbe[i].userGuid, 
                        email: this.usersbe[i].username, 
                        password: this.usersbe[i].password, 
                        rememberMe: this.rememberMe 
                    };
                    break; // beendet die Schleife, wenn der Benutzer gefunden ist
                }
            }

            // Überprüfe, ob ein Benutzer gefunden wurde
            if (foundUser) {
                this.guid = foundUser.guid; // Aktualisiere die Komponenten-guid
                this.login(foundUser);
                console.log(this.guid, this.email, this.password, this.rememberMe);
                this.$router.push('/offers');
            } else {
                console.log("Benutzer nicht gefunden");
            }
        }
    }
}
</script>

<template>
    <div class="flex align-items-center justify-content-center" style="height: 100vh;">
        <div class="surface-card p-4 shadow-2 border-round w-full lg:w-6">
            <div class="text-center mb-5">
                <img src="../../public/images/Anikatze.png" alt="Image" height="50" class="mb-3" />
                <div class="text-900 text-3xl font-medium mb-3">Welcome Back</div>
                <span class="text-600 font-medium line-height-3">Don't have an account?</span>
                <a class="font-medium no-underline ml-2 text-blue-500 cursor-pointer">Create today!</a>
            </div>
            <div>
                <label for="email1" class="block text-900 font-medium mb-2">Email</label>
                <InputText id="email1" type="text" placeholder="Email address" v-model="email" class="w-full mb-3" />

                <label for="password1" class="block text-900 font-medium mb-2">Password</label>
                <InputText id="password1" type="password" placeholder="Password" v-model="password" class="w-full mb-3" />

                <div class="flex align-items-center justify-content-between mb-6">
                    <div class="flex align-items-center">
                        <Checkbox id="rememberme1" :binary="true" v-model="checked1" class="mr-2"></Checkbox>
                        <label for="rememberme1">Remember me</label>
                    </div>
                    <a class="font-medium no-underline ml-2 text-blue-500 text-right cursor-pointer">Forgot password?</a>
                </div>
                <Button label="Sign In" icon="pi pi-user" class="w-full" @click="performLogin"></Button>
            </div>
        </div>
    </div>
</template>

<style>
</style>
