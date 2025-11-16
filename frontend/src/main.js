import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';
import { startConnection } from './services/signalr';

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);

// Start SignalR connection if authenticated
const token = localStorage.getItem('token');
if (token) {
  startConnection();
}

app.mount('#app');
