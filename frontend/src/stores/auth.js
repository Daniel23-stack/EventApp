import { defineStore } from 'pinia';
import api from '../services/api';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('user') || 'null'),
    token: localStorage.getItem('token') || null
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    userRole: (state) => state.user?.role || null
  },

  actions: {
    async login(credentials) {
      try {
        const response = await api.post('/auth/login', credentials);
        this.token = response.data.token;
        this.user = {
          username: response.data.username,
          email: response.data.email,
          role: response.data.role
        };
        localStorage.setItem('token', this.token);
        localStorage.setItem('user', JSON.stringify(this.user));
        return { success: true };
      } catch (error) {
        return { success: false, error: error.response?.data?.message || 'Login failed' };
      }
    },

    async register(userData) {
      try {
        const response = await api.post('/auth/register', userData);
        this.token = response.data.token;
        this.user = {
          username: response.data.username,
          email: response.data.email,
          role: response.data.role
        };
        localStorage.setItem('token', this.token);
        localStorage.setItem('user', JSON.stringify(this.user));
        return { success: true };
      } catch (error) {
        return { success: false, error: error.response?.data?.message || 'Registration failed' };
      }
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }
  }
});

