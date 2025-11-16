<template>
  <div>
    <h1>Analytics Dashboard</h1>
    
    <div class="analytics-grid">
      <div class="card">
        <h2>ABC Analysis</h2>
        <table>
          <thead>
            <tr>
              <th>Category</th>
              <th>Product</th>
              <th>Total Value</th>
              <th>Percentage</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in abcAnalysis" :key="item.productId">
              <td>
                <span :class="`abc-category abc-${item.category.toLowerCase()}`">
                  {{ item.category }}
                </span>
              </td>
              <td>{{ item.productName }}</td>
              <td>${{ item.totalValue.toFixed(2) }}</td>
              <td>{{ item.percentageOfTotal.toFixed(2) }}%</td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <div class="card">
        <h2>Turnover Rates</h2>
        <table>
          <thead>
            <tr>
              <th>Product</th>
              <th>Turnover Rate</th>
              <th>Avg Inventory</th>
              <th>Total Sales</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in turnoverRates" :key="item.productId">
              <td>{{ item.productName }}</td>
              <td>{{ item.turnoverRate.toFixed(2) }}</td>
              <td>{{ item.averageInventory }}</td>
              <td>{{ item.totalSales }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <div class="card">
        <h2>Product Performance</h2>
        <table>
          <thead>
            <tr>
              <th>Product</th>
              <th>Total Sales</th>
              <th>Revenue</th>
              <th>Stock Value</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in productPerformance" :key="item.productId">
              <td>{{ item.productName }}</td>
              <td>{{ item.totalSales }}</td>
              <td>${{ item.totalRevenue.toFixed(2) }}</td>
              <td>${{ item.stockValue.toFixed(2) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    
    <div class="card">
      <h2>Sales Forecast</h2>
      <div class="form-group">
        <label>Select Product</label>
        <select v-model="selectedProductId" @change="loadForecast">
          <option value="">Select Product</option>
          <option v-for="product in products" :key="product.id" :value="product.id">
            {{ product.name }}
          </option>
        </select>
      </div>
      <div v-if="salesForecast.length > 0" class="forecast-chart">
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Forecasted Sales</th>
              <th>Confidence</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="forecast in salesForecast" :key="forecast.date">
              <td>{{ formatDate(forecast.date) }}</td>
              <td>${{ forecast.forecastedSales.toFixed(2) }}</td>
              <td>{{ (forecast.confidence * 100).toFixed(0) }}%</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';

const abcAnalysis = ref([]);
const turnoverRates = ref([]);
const productPerformance = ref([]);
const salesForecast = ref([]);
const products = ref([]);
const selectedProductId = ref('');

const loadABCAnalysis = async () => {
  try {
    const response = await api.get('/analytics/abc-analysis');
    abcAnalysis.value = response.data;
  } catch (error) {
    console.error('Error loading ABC analysis:', error);
  }
};

const loadTurnoverRates = async () => {
  try {
    const response = await api.get('/analytics/turnover-rates');
    turnoverRates.value = response.data;
  } catch (error) {
    console.error('Error loading turnover rates:', error);
  }
};

const loadProductPerformance = async () => {
  try {
    const response = await api.get('/analytics/product-performance');
    productPerformance.value = response.data;
  } catch (error) {
    console.error('Error loading product performance:', error);
  }
};

const loadProducts = async () => {
  try {
    const response = await api.get('/products');
    products.value = response.data;
  } catch (error) {
    console.error('Error loading products:', error);
  }
};

const loadForecast = async () => {
  if (!selectedProductId.value) return;
  try {
    const response = await api.get(`/analytics/sales-forecast/${selectedProductId.value}?days=30`);
    salesForecast.value = response.data;
  } catch (error) {
    console.error('Error loading sales forecast:', error);
  }
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString();
};

onMounted(() => {
  loadABCAnalysis();
  loadTurnoverRates();
  loadProductPerformance();
  loadProducts();
});
</script>

<style scoped>
.analytics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.abc-category {
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-weight: bold;
}

.abc-a {
  background-color: #27ae60;
  color: white;
}

.abc-b {
  background-color: #f39c12;
  color: white;
}

.abc-c {
  background-color: #95a5a6;
  color: white;
}

.forecast-chart {
  margin-top: 1rem;
}
</style>

