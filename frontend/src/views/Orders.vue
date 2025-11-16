<template>
  <div>
    <div class="page-header">
      <h1>Orders</h1>
      <button @click="showCreateModal = true" class="btn btn-primary">Create Order</button>
    </div>
    
    <div class="card">
      <table>
        <thead>
          <tr>
            <th>Order ID</th>
            <th>Supplier</th>
            <th>Order Date</th>
            <th>Status</th>
            <th>Total Amount</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="order in orders" :key="order.id">
            <td>{{ order.id }}</td>
            <td>{{ order.supplierName }}</td>
            <td>{{ formatDate(order.orderDate) }}</td>
            <td>
              <span :class="`status-badge status-${order.status.toLowerCase()}`">
                {{ order.status }}
              </span>
            </td>
            <td>${{ order.totalAmount.toFixed(2) }}</td>
            <td>
              <button @click="viewOrder(order)" class="btn btn-primary">View</button>
              <button v-if="order.status === 'Pending'" @click="updateStatus(order.id, 'Confirmed')" class="btn">Confirm</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <!-- Create Order Modal -->
    <div v-if="showCreateModal" class="modal-overlay" @click="showCreateModal = false">
      <div class="modal-content" @click.stop>
        <h2>Create Order</h2>
        <form @submit.prevent="handleCreateOrder">
          <div class="form-group">
            <label>Supplier</label>
            <select v-model="orderForm.supplierId" required>
              <option value="">Select Supplier</option>
              <option v-for="supplier in suppliers" :key="supplier.id" :value="supplier.id">
                {{ supplier.name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Items</label>
            <div v-for="(item, index) in orderForm.items" :key="index" class="order-item">
              <select v-model="item.productId" required>
                <option value="">Select Product</option>
                <option v-for="product in products" :key="product.id" :value="product.id">
                  {{ product.name }} - ${{ product.unitPrice.toFixed(2) }}
                </option>
              </select>
              <input v-model.number="item.quantity" type="number" placeholder="Quantity" required />
              <input v-model.number="item.unitPrice" type="number" step="0.01" placeholder="Unit Price" required />
              <button type="button" @click="orderForm.items.splice(index, 1)" class="btn btn-danger">Remove</button>
            </div>
            <button type="button" @click="orderForm.items.push({ productId: '', quantity: 1, unitPrice: 0 })" class="btn">
              Add Item
            </button>
          </div>
          <div class="modal-actions">
            <button type="submit" class="btn btn-primary">Create Order</button>
            <button type="button" @click="showCreateModal = false" class="btn">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';

const orders = ref([]);
const suppliers = ref([]);
const products = ref([]);
const showCreateModal = ref(false);
const orderForm = ref({
  supplierId: '',
  items: [{ productId: '', quantity: 1, unitPrice: 0 }]
});

const loadOrders = async () => {
  try {
    const response = await api.get('/orders');
    orders.value = response.data;
  } catch (error) {
    console.error('Error loading orders:', error);
  }
};

const loadSuppliers = async () => {
  try {
    const response = await api.get('/suppliers');
    suppliers.value = response.data;
  } catch (error) {
    console.error('Error loading suppliers:', error);
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

const handleCreateOrder = async () => {
  try {
    await api.post('/orders', orderForm.value);
    showCreateModal.value = false;
    orderForm.value = { supplierId: '', items: [{ productId: '', quantity: 1, unitPrice: 0 }] };
    loadOrders();
  } catch (error) {
    console.error('Error creating order:', error);
    alert('Error creating order');
  }
};

const updateStatus = async (orderId, status) => {
  try {
    await api.put(`/orders/${orderId}/status`, { status });
    loadOrders();
  } catch (error) {
    console.error('Error updating order status:', error);
    alert('Error updating order status');
  }
};

const viewOrder = (order) => {
  alert(`Order Details:\nSupplier: ${order.supplierName}\nItems: ${order.items.length}\nTotal: $${order.totalAmount.toFixed(2)}`);
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString();
};

onMounted(() => {
  loadOrders();
  loadSuppliers();
  loadProducts();
});
</script>

<style scoped>
.order-item {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  align-items: center;
}

.order-item select,
.order-item input {
  flex: 1;
}

.status-badge {
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
  font-weight: 500;
}

.status-pending {
  background-color: #f39c12;
  color: white;
}

.status-confirmed {
  background-color: #3498db;
  color: white;
}

.status-shipped {
  background-color: #9b59b6;
  color: white;
}

.status-delivered {
  background-color: #27ae60;
  color: white;
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
  max-width: 600px;
  max-height: 90vh;
  overflow-y: auto;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 1rem;
}
</style>

