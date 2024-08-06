<script>
import Sidebar from 'primevue/sidebar';
import Button from 'primevue/button';

export default {
  components: {
    Sidebar,
    Button
  },
  props: {
    visible: {
      type: Boolean,
      required: true
    },
    lections: {
      type: Array,
      required: true
    }
  },
  methods: {
    updateVisible(newValue) {
      this.$emit('update:visible', newValue);  // Emits the value to the parent component
      console.log('Closing sidebar');
    },
    closeSidebar() {
      this.updateVisible(false);  // Closes the sidebar
      console.log('Closing sidebar 2');
    },
    selectLection(lection){
      console.log(lection);
      const courseGuid = this.$route.params.courseGuid;
      this.$router.push(`/${courseGuid}/${lection.lectionGuid}`);
    }
  }
};
</script>

<template>
  <div class="card flex justify-content-center">
    <Sidebar :visible="visible" @update:visible="updateVisible" title="hi">
      <template #container="{closeCallback}">
        <div class="flex flex-column h-full">
          <div class="flex align-items-center justify-content-between px-4 pt-3 flex-shrink-0">
            <span>
              <Button
                type="button"
                @click="closeSidebar"
                icon="pi pi-times"
                rounded
                outlined
                class="h-2rem w-2rem"
              ></Button>
            </span>
          </div>
          <div class="overflow-y-auto">
            <ul class="list-none p-3 m-0">
              <li>
                <ul class="list-none p-0 m-0 overflow-hidden">
                  <li v-for="lection in lections" :key="lection.lectionGuid">
                    <a
                      v-ripple
                      @click="selectLection(lection)"
                      class="flex align-items-center cursor-pointer p-3 border-round text-700 hover:surface-100 transition-duration-150 transition-colors p-ripple"
                    >
                      <i class="pi pi-home mr-2"></i>
                      <span class="font-medium">{{lection.title}}</span>
                    </a>
                  </li>
                </ul>
              </li>
            </ul>
          </div>
        </div>
      </template>
    </Sidebar>
  </div>
</template>
