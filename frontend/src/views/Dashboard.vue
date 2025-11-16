<template>
  <div>
    <h1>Dashboard</h1>
    <div class="stats-grid">
      <div class="stat-card">
        <h3>Total Products</h3>
        <p class="stat-value">{{ stats.totalProducts }}</p>
      </div>
      <div class="stat-card">
        <h3>Low Stock Items</h3>
        <p class="stat-value warning">{{ stats.lowStockItems }}</p>
      </div>
      <div class="stat-card">
        <h3>Active Alerts</h3>
        <p class="stat-value danger">{{ stats.activeAlerts }}</p>
      </div>
      <div class="stat-card">
        <h3>Total Suppliers</h3>
        <p class="stat-value">{{ stats.totalSuppliers }}</p>
      </div>
    </div>
    
    <div class="dashboard-grid">
      <div class="card">
        <h2>Recent Alerts</h2>
        <div v-if="recentAlerts.length === 0" class="empty-state">No alerts</div>
        <ul v-else class="alert-list">
          <li v-for="alert in recentAlerts" :key="alert.id" class="alert-item">
            <span class="alert-type">{{ alert.type }}</span>
            <span class="alert-message">{{ alert.message }}</span>
            <span class="alert-date">{{ formatDate(alert.createdAt) }}</span>
          </li>
        </ul>
      </div>
      
      <div class="card">
        <h2>Quick Actions</h2>
        <div class="quick-actions">
          <router-link to="/products" class="action-btn">Manage Products</router-link>
          <router-link to="/inventory" class="action-btn">View Inventory</router-link>
          <router-link to="/orders" class="action-btn">Create Order</router-link>
          <router-link to="/analytics" class="action-btn">View Analytics</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';
import { onNewAlert } from '../services/signalr';

const stats = ref({
  totalProducts: 0,
  lowStockItems: 0,
  activeAlerts: 0,
  totalSuppliers: 0
});

const recentAlerts = ref([]);

const loadDashboard = async () => {
  try {
    const [products, lowStock, alerts, suppliers] = await Promise.all([
      api.get('/products'),
      api.get('/inventory/low-stock'),
      api.get('/alerts'),
      api.get('/suppliers')
    ]);
    
    stats.value = {
      totalProducts: products.data.length,
      lowStockItems: lowStock.data.length,
      activeAlerts: alerts.data.length,
      totalSuppliers: suppliers.data.length
    };
    
    recentAlerts.value = alerts.data.slice(0, 5);
  } catch (error) {
    console.error('Error loading dashboard:', error);
  }
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString();
};

onMounted(() => {
  loadDashboard();
  
  onNewAlert(() => {
    loadDashboard();
  });
});
</script>

<style scoped>
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.stat-card h3 {
  font-size: 0.9rem;
  color: #666;
  margin-bottom: 0.5rem;
}

.stat-value {
  font-size: 2rem;
  font-weight: bold;
  color: #2c3e50;
}

.stat-value.warning {
  color: #f39c12;
}

.stat-value.danger {
  color: #e74c3c;
}

.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 1.5rem;
}

.alert-list {
  list-style: none;
}

.alert-item {
  padding: 0.75rem;
  border-bottom: 1px solid #eee;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.alert-type {
  font-size: 0.8rem;
  color: #666;
  text-transform: uppercase;
}

.alert-message {
  font-weight: 500;
}

.alert-date {
  font-size: 0.8rem;
  color: #999;
}

.quick-actions {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.action-btn {
  padding: 0.75rem;
  background-color: #3498db;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  text-align: center;
  transition: background-color 0.3s;
}

.action-btn:hover {
  background-color: #2980b9;
}

.empty-state {
  text-align: center;
  color: #999;
  padding: 2rem;
}
</style>

