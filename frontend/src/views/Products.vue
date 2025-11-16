<template>
  <div>
    <div class="page-header">
      <h1>Products</h1>
      <button @click="showCreateModal = true" class="btn btn-primary">Add Product</button>
    </div>
    
    <div class="card">
      <div class="search-bar">
        <input v-model="searchTerm" @input="searchProducts" type="text" placeholder="Search products..." />
      </div>
      
      <table>
        <thead>
          <tr>
            <th>SKU</th>
            <th>Name</th>
            <th>Category</th>
            <th>Unit Price</th>
            <th>Reorder Level</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in products" :key="product.id">
            <td>{{ product.sku }}</td>
            <td>{{ product.name }}</td>
            <td>{{ product.category || 'N/A' }}</td>
            <td>${{ product.unitPrice.toFixed(2) }}</td>
            <td>{{ product.reorderLevel }}</td>
            <td>
              <button @click="editProduct(product)" class="btn btn-primary">Edit</button>
              <button @click="deleteProduct(product.id)" class="btn btn-danger">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <!-- Create/Edit Product Modal -->
    <div v-if="showCreateModal || editingProduct" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>{{ editingProduct ? 'Edit Product' : 'Add Product' }}</h2>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>SKU</label>
            <input v-model="productForm.sku" type="text" required :disabled="!!editingProduct" />
          </div>
          <div class="form-group">
            <label>Name</label>
            <input v-model="productForm.name" type="text" required />
          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea v-model="productForm.description"></textarea>
          </div>
          <div class="form-group">
            <label>Category</label>
            <input v-model="productForm.category" type="text" />
          </div>
          <div class="form-group">
            <label>Unit Price</label>
            <input v-model.number="productForm.unitPrice" type="number" step="0.01" required />
          </div>
          <div class="form-group">
            <label>Reorder Level</label>
            <input v-model.number="productForm.reorderLevel" type="number" required />
          </div>
          <div class="modal-actions">
            <button type="submit" class="btn btn-primary">Save</button>
            <button type="button" @click="closeModal" class="btn">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';

const products = ref([]);
const searchTerm = ref('');
const showCreateModal = ref(false);
const editingProduct = ref(null);
const productForm = ref({
  sku: '',
  name: '',
  description: '',
  category: '',
  unitPrice: 0,
  reorderLevel: 0
});

const loadProducts = async () => {
  try {
    const response = await api.get('/products');
    products.value = response.data;
  } catch (error) {
    console.error('Error loading products:', error);
  }
};

const searchProducts = async () => {
  if (!searchTerm.value) {
    loadProducts();
    return;
  }
  try {
    const response = await api.get(`/products/search?term=${searchTerm.value}`);
    products.value = response.data;
  } catch (error) {
    console.error('Error searching products:', error);
  }
};

const handleSubmit = async () => {
  try {
    if (editingProduct.value) {
      await api.put(`/products/${editingProduct.value.id}`, productForm.value);
    } else {
      await api.post('/products', productForm.value);
    }
    closeModal();
    loadProducts();
  } catch (error) {
    console.error('Error saving product:', error);
    alert('Error saving product');
  }
};

const editProduct = (product) => {
  editingProduct.value = product;
  productForm.value = { ...product };
};

const deleteProduct = async (id) => {
  if (!confirm('Are you sure you want to delete this product?')) return;
  try {
    await api.delete(`/products/${id}`);
    loadProducts();
  } catch (error) {
    console.error('Error deleting product:', error);
    alert('Error deleting product');
  }
};

const closeModal = () => {
  showCreateModal.value = false;
  editingProduct.value = null;
  productForm.value = {
    sku: '',
    name: '',
    description: '',
    category: '',
    unitPrice: 0,
    reorderLevel: 0
  };
};

onMounted(() => {
  loadProducts();
});
</script>

<style scoped>
.search-bar {
  margin-bottom: 1rem;
}

.search-bar input {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
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

