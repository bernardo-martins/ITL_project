<script>
import Menubar from 'primevue/menubar';
import InputText from 'primevue/inputtext';
import Badge from 'primevue/badge';
import 'primeicons/primeicons.css';
import { mapGetters } from 'vuex';

export default {
  components: {
    Menubar,
    InputText,
    Badge
  },
  async mounted() {
    const userGuid = this.getUserGuid();
  },
  computed: {

    
    ...mapGetters({isAuthenticated: 'isAuthenticated',  getUser : "getUser"}),
    menuItems() {
      let items = [
        { label: 'Offers', icon: 'pi pi-shopping-bag', command: () => this.$router.push('/Offers') },
        { label: 'Mein Account', icon: 'pi pi-user', command: () => this.navigateToAccount() },
        { label: 'My Courses', icon: 'pi pi-graduation-cap', command: () => this.$router.push('/UserCourses') },
        {icon: 'pi pi-shopping-bag', command: () => this.$router.push('/Cart') },
      ];

      
      if (this.$route.name === "CoursePage") {
        items.push({
          label: 'Lections',
          icon: 'pi pi-book',
          command: () => this.$emit('toggle-sidebar')
        });
      }
      
      if (!this.isAuthenticated) {
        items.push({ label: 'Sign-in', icon: 'pi pi-sign-in', command: () => this.$router.push('/LoginPage') });
      } else {
        items.push({ label: 'Logout', icon: 'pi pi-sign-out', command: () => {
          this.$store.dispatch('logout');
          this.$router.push('/LoginPage');
        } });
      }

      return items;
    }
  },
  methods: {
    getUserGuid() {
      return this.getUser ? this.getUser.guid : null;
    },
    navigateToAccount() {
      const userGuid = this.getUserGuid();
      if (userGuid) {
        this.$router.push(`/MyAccount/${userGuid}`);
      } else {
        console.error('User ID is undefined');
      }
    }
  }
}
</script>



<template>
    <header>
        <div class="card">
            <Menubar :model="menuItems">
                <template #start>
                    <img src="/public/images/Anikatze.png" alt="Logo" width="35" height="40" class="h-2rem">
                </template>
                <template #item="{ item, props, hasSubmenu, root }">
                    <a v-ripple class="flex align-items-center" v-bind="props.action" @click.prevent.stop="item.command && item.command($event)">
                        <span :class="item.icon" />
                        <span class="ml-2">{{ item.label }}</span>
                        <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
                        <span v-if="item.shortcut" class="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{{ item.shortcut }}</span>
                        <i v-if="hasSubmenu" :class="['pi pi-angle-down', { 'pi-angle-down ml-2': root, 'pi-angle-right ml-auto': !root }]"></i>
                    </a>
                    

                </template>
                <template #end>

            </template>
        </Menubar>
    </div>
  </header>
</template>


<style>
* {
    font-family: "museo-sans-rounded", sans-serif;
    font-weight: 900;
    font-style: normal;
  }
</style>