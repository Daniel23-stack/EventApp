<template>
  <div>
    <div class="page-header">
      <h1>Inventory Management</h1>
      <button @click="showAdjustModal = true" class="btn btn-primary">Adjust Inventory</button>
    </div>
    
    <div class="card">
      <table>
        <thead>
          <tr>
            <th>Product</th>
            <th>SKU</th>
            <th>Quantity</th>
            <th>Location</th>
            <th>Last Updated</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in inventory" :key="item.id">
            <td>{{ item.productName }}</td>
            <td>{{ item.productSKU }}</td>
            <td :class="{ 'low-stock': isLowStock(item) }">{{ item.quantity }}</td>
            <td>{{ item.location || 'N/A' }}</td>
            <td>{{ formatDate(item.lastUpdated) }}</td>
            <td>
              <button @click="editInventory(item)" class="btn btn-primary">Edit</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <!-- Adjust Inventory Modal -->
    <div v-if="showAdjustModal" class="modal-overlay" @click="showAdjustModal = false">
      <div class="modal-content" @click.stop>
        <h2>Adjust Inventory</h2>
        <form @submit.prevent="handleAdjust">
          <div class="form-group">
            <label>Product</label>
            <select v-model="adjustForm.productId" required>
              <option value="">Select Product</option>
              <option v-for="product in products" :key="product.id" :value="product.id">
                {{ product.name }} ({{ product.sku }})
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Change Type</label>
            <select v-model="adjustForm.changeType" required>
              <option value="Purchase">Purchase</option>
              <option value="Sale">Sale</option>
              <option value="Adjustment">Adjustment</option>
              <option value="Return">Return</option>
            </select>
          </div>
          <div class="form-group">
            <label>Change Amount</label>
            <input v-model.number="adjustForm.changeAmount" type="number" required />
          </div>
          <div class="form-group">
            <label>Location</label>
            <input v-model="adjustForm.location" type="text" />
          </div>
          <div class="modal-actions">
            <button type="submit" class="btn btn-primary">Adjust</button>
            <button type="button" @click="showAdjustModal = false" class="btn">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import api from '../services/api';
import { onInventoryUpdated, offInventoryUpdated } from '../services/signalr';

const inventory = ref([]);
const products = ref([]);
const showAdjustModal = ref(false);
const adjustForm = ref({
  productId: '',
  changeType: 'Adjustment',
  changeAmount: 0,
  location: ''
});

const loadInventory = async () => {
  try {
    const response = await api.get('/inventory');
    inventory.value = response.data;
  } catch (error) {
    console.error('Error loading inventory:', error);
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

const handleAdjust = async () => {
  try {
    await api.post('/inventory/adjust', adjustForm.value);
    showAdjustModal.value = false;
    adjustForm.value = { productId: '', changeType: 'Adjustment', changeAmount: 0, location: '' };
    loadInventory();
  } catch (error) {
    console.error('Error adjusting inventory:', error);
    alert('Error adjusting inventory');
  }
};

const editInventory = (item) => {
  // Implement edit functionality
  console.log('Edit inventory:', item);
};

const isLowStock = (item) => {
  return item.quantity <= 10; // Simple threshold
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleString();
};

const handleInventoryUpdate = (updatedItem) => {
  const index = inventory.value.findIndex(i => i.id === updatedItem.id);
  if (index !== -1) {
    inventory.value[index] = updatedItem;
  } else {
    inventory.value.push(updatedItem);
  }
};

onMounted(() => {
  loadInventory();
  loadProducts();
  onInventoryUpdated(handleInventoryUpdate);
});

onUnmounted(() => {
  offInventoryUpdated();
});
</script>

<style scoped>
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.low-stock {
  color: #e74c3c;
  font-weight: bold;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  width: 90%;
  max-width: 500px;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 1rem;
}
</style>

