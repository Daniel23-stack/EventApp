<template>
  <div class="register-container">
    <div class="register-card">
      <h1>Register</h1>
      <form @submit.prevent="handleRegister">
        <div class="form-group">
          <label>Username</label>
          <input v-model="userData.username" type="text" required />
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="userData.email" type="email" required />
        </div>
        <div class="form-group">
          <label>Password</label>
          <input v-model="userData.password" type="password" required />
        </div>
        <div class="form-group">
          <label>Role</label>
          <select v-model="userData.role">
            <option value="User">User</option>
            <option value="Manager">Manager</option>
            <option value="Admin">Admin</option>
          </select>
        </div>
        <div v-if="error" class="error-message">{{ error }}</div>
        <button type="submit" class="btn btn-primary" :disabled="loading">
          {{ loading ? 'Registering...' : 'Register' }}
        </button>
        <p class="login-link">
          Already have an account? <router-link to="/login">Login here</router-link>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';
import { startConnection } from '../services/signalr';

const router = useRouter();
const authStore = useAuthStore();

const userData = ref({
  username: '',
  email: '',
  password: '',
  role: 'User'
});

const loading = ref(false);
const error = ref('');

const handleRegister = async () => {
  loading.value = true;
  error.value = '';
  
  const result = await authStore.register(userData.value);
  
  if (result.success) {
    startConnection();
    router.push('/dashboard');
  } else {
    error.value = result.error;
  }
  
  loading.value = false;
};
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.register-card {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
  width: 100%;
  max-width: 400px;
}

.register-card h1 {
  margin-bottom: 1.5rem;
  text-align: center;
  color: #2c3e50;
}

.error-message {
  color: #e74c3c;
  margin-bottom: 1rem;
  padding: 0.5rem;
  background-color: #fee;
  border-radius: 4px;
}

.login-link {
  margin-top: 1rem;
  text-align: center;
}

.login-link a {
  color: #3498db;
  text-decoration: none;
}

.login-link a:hover {
  text-decoration: underline;
}
</style>

