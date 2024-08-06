import { createStore } from 'vuex';

const store = createStore({
  state() {
    return {
      isAuthenticated: false,
      user: {
        id: 0,
        name: '',
        guid: '',
      }

    };
  },
  mutations: {
    setAuthUser(state, user) {
      state.isAuthenticated = true;
      state.user = user;
    },
    clearAuthUser(state) {
      state.isAuthenticated = false;
      state.user = {
        id: null,
        email: '',
        guid: '' // Reset guid on logout
      }
    }
  },
  actions: {
    login({ commit }, user) {
      commit('setAuthUser', user);
    },
    logout({ commit }) {
      commit('clearAuthUser');
    }
  },
  getters: {
    isAuthenticated: state => state.isAuthenticated,
    getUser: state => state.user
  }
});

export default store;