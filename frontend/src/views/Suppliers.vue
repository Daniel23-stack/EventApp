<template>
  <div>
    <div class="page-header">
      <h1>Suppliers</h1>
      <button @click="showCreateModal = true" class="btn btn-primary">Add Supplier</button>
    </div>
    
    <div class="card">
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Contact</th>
            <th>Email</th>
            <th>Phone</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="supplier in suppliers" :key="supplier.id">
            <td>{{ supplier.name }}</td>
            <td>{{ supplier.contactName || 'N/A' }}</td>
            <td>{{ supplier.email || 'N/A' }}</td>
            <td>{{ supplier.phone || 'N/A' }}</td>
            <td>
              <button @click="editSupplier(supplier)" class="btn btn-primary">Edit</button>
              <button @click="deleteSupplier(supplier.id)" class="btn btn-danger">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <!-- Create/Edit Supplier Modal -->
    <div v-if="showCreateModal || editingSupplier" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>{{ editingSupplier ? 'Edit Supplier' : 'Add Supplier' }}</h2>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>Name</label>
            <input v-model="supplierForm.name" type="text" required />
          </div>
          <div class="form-group">
            <label>Contact Name</label>
            <input v-model="supplierForm.contactName" type="text" />
          </div>
          <div class="form-group">
            <label>Email</label>
            <input v-model="supplierForm.email" type="email" />
          </div>
          <div class="form-group">
            <label>Phone</label>
            <input v-model="supplierForm.phone" type="text" />
          </div>
          <div class="form-group">
            <label>Address</label>
            <textarea v-model="supplierForm.address"></textarea>
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

const suppliers = ref([]);
const showCreateModal = ref(false);
const editingSupplier = ref(null);
const supplierForm = ref({
  name: '',
  contactName: '',
  email: '',
  phone: '',
  address: ''
});

const loadSuppliers = async () => {
  try {
    const response = await api.get('/suppliers');
    suppliers.value = response.data;
  } catch (error) {
    console.error('Error loading suppliers:', error);
  }
};

const handleSubmit = async () => {
  try {
    if (editingSupplier.value) {
      await api.put(`/suppliers/${editingSupplier.value.id}`, supplierForm.value);
    } else {
      await api.post('/suppliers', supplierForm.value);
    }
    closeModal();
    loadSuppliers();
  } catch (error) {
    console.error('Error saving supplier:', error);
    alert('Error saving supplier');
  }
};

const editSupplier = (supplier) => {
  editingSupplier.value = supplier;
  supplierForm.value = { ...supplier };
};

const deleteSupplier = async (id) => {
  if (!confirm('Are you sure you want to delete this supplier?')) return;
  try {
    await api.delete(`/suppliers/${id}`);
    loadSuppliers();
  } catch (error) {
    console.error('Error deleting supplier:', error);
    alert('Error deleting supplier');
  }
};

const closeModal = () => {
  showCreateModal.value = false;
  editingSupplier.value = null;
  supplierForm.value = {
    name: '',
    contactName: '',
    email: '',
    phone: '',
    address: ''
  };
};

onMounted(() => {
  loadSuppliers();
});
</script>

<style scoped>
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

