<template>
  <div>
    <div class="page-header">
      <h1>Alerts</h1>
      <button @click="checkReorderLevels" class="btn btn-primary">Check Reorder Levels</button>
    </div>
    
    <div class="card">
      <table>
        <thead>
          <tr>
            <th>Type</th>
            <th>Product</th>
            <th>Message</th>
            <th>Created</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="alert in alerts" :key="alert.id">
            <td>
              <span :class="`alert-type alert-${alert.type.toLowerCase()}`">
                {{ alert.type }}
              </span>
            </td>
            <td>{{ alert.productName || 'N/A' }}</td>
            <td>{{ alert.message }}</td>
            <td>{{ formatDate(alert.createdAt) }}</td>
            <td>
              <span :class="alert.isResolved ? 'resolved' : 'active'">
                {{ alert.isResolved ? 'Resolved' : 'Active' }}
              </span>
            </td>
            <td>
              <button 
                v-if="!alert.isResolved" 
                @click="resolveAlert(alert.id)" 
                class="btn btn-primary"
              >
                Resolve
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import api from '../services/api';
import { onNewAlert, offNewAlert } from '../services/signalr';

const alerts = ref([]);

const loadAlerts = async () => {
  try {
    const response = await api.get('/alerts');
    alerts.value = response.data;
  } catch (error) {
    console.error('Error loading alerts:', error);
  }
};

const resolveAlert = async (alertId) => {
  try {
    await api.post(`/alerts/${alertId}/resolve`);
    loadAlerts();
  } catch (error) {
    console.error('Error resolving alert:', error);
    alert('Error resolving alert');
  }
};

const checkReorderLevels = async () => {
  try {
    await api.post('/alerts/check-reorder-levels');
    alert('Reorder levels checked successfully');
    loadAlerts();
  } catch (error) {
    console.error('Error checking reorder levels:', error);
    alert('Error checking reorder levels');
  }
};

const handleNewAlert = (newAlert) => {
  alerts.value.unshift(newAlert);
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleString();
};

onMounted(() => {
  loadAlerts();
  onNewAlert(handleNewAlert);
});

onUnmounted(() => {
  offNewAlert();
});
</script>

<style scoped>
.alert-type {
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
  font-weight: 500;
  text-transform: uppercase;
}

.alert-lowstock {
  background-color: #f39c12;
  color: white;
}

.alert-reorder {
  background-color: #e74c3c;
  color: white;
}

.resolved {
  color: #27ae60;
  font-weight: 500;
}

.active {
  color: #e74c3c;
  font-weight: 500;
}
</style>

