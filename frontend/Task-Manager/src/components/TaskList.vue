<template>
  <div class="container mt-4">
    <h1>Task Manager</h1>
    
    <!-- Create/Edit Task Form -->
    <div class="card mb-4">
      <div class="card-body">
        <h5 class="card-title">{{ editMode ? 'Edit Task' : 'Add New Task' }}</h5>
        <form @submit.prevent="editMode ? updateEditedTask() : addTask()">
          <div class="mb-3">
            <label for="title" class="form-label">Title</label>
            <input 
              v-model="newTask.title" 
              type="text" 
              class="form-control" 
              id="title" 
              required
            >
          </div>
          <div class="mb-3">
            <label for="description" class="form-label">Description</label>
            <textarea 
              v-model="newTask.description" 
              class="form-control" 
              id="description" 
              rows="3"
            ></textarea>
          </div>
          <div class="d-flex gap-2">
            <button type="submit" class="btn btn-primary">{{ editMode ? 'Update Task' : 'Add Task' }}</button>
            <button v-if="editMode" type="button" @click="cancelEdit" class="btn btn-secondary">Cancel</button>
          </div>
        </form>
      </div>
    </div>
    
    <!-- Task List -->
    <div v-if="loading" class="text-center">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
    
    <div v-else-if="error" class="alert alert-danger" role="alert">
      {{ error }}
    </div>
    
    <div v-else-if="tasks.length === 0" class="alert alert-info" role="alert">
      No tasks available. Add a new task to get started!
    </div>
    
    <div v-else>
      <div class="row">
        <div v-for="task in tasks" :key="task.id" class="col-md-6 mb-3">
          <div class="card h-100">
            <div class="card-body">
              <div class="d-flex justify-content-between align-items-center">
                <h5 class="card-title">
                  <span :class="{ 'text-decoration-line-through': task.completed }">
                    {{ task.title }}
                  </span>
                </h5>
                <div class="form-check">
                  <input 
                    :id="'completed-' + task.id" 
                    class="form-check-input" 
                    type="checkbox" 
                    v-model="task.completed"
                    @change="toggleTaskStatus(task)"
                  >
                  <label class="form-check-label" :for="'completed-' + task.id">
                    Completed
                  </label>
                </div>
              </div>
              <p class="card-text">{{ task.description }}</p>
              <div class="d-flex justify-content-end gap-2">
                <button @click="editTask(task)" class="btn btn-primary btn-sm">
                  Edit
                </button>
                <button @click="deleteTask(task.id)" class="btn btn-danger btn-sm">
                  Delete
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'TaskList',
  data() {
    return {
      tasks: [],
      newTask: {
        title: '',
        description: '',
        completed: false
      },
      editMode: false,
      editTaskId: null,
      loading: true,
      error: null,
      apiUrl: 'http://localhost:5001/api/tasks'
    };
  },
  mounted() {
    this.fetchTasks();
  },
  methods: {
    async fetchTasks() {
      this.loading = true;
      this.error = null;
      
      try {
        const response = await axios.get(this.apiUrl);
        this.tasks = response.data;
      } catch (err) {
        console.error('Error fetching tasks:', err);
        this.error = 'Failed to load tasks. Please try again later.';
      } finally {
        this.loading = false;
      }
    },
    
    async addTask() {
      try {
        console.log('Attempting to add task:', this.newTask);
        console.log('API URL:', this.apiUrl);
        const response = await axios.post(this.apiUrl, this.newTask);
        console.log('Response received:', response.data);
        this.tasks.push(response.data);
        this.resetForm();
      } catch (err) {
        console.error('Error adding task:', err);
        console.error('Error details:', err.response ? err.response.data : 'No response data');
        console.error('Status code:', err.response ? err.response.status : 'No status code');
        alert('Failed to add task. Please check the console for details.');
      }
    },
    
    editTask(task) {
      this.editMode = true;
      this.editTaskId = task.id;
      this.newTask = {
        id: task.id,
        title: task.title,
        description: task.description,
        completed: task.completed
      };
    },
    
    async updateEditedTask() {
      try {
        const response = await axios.put(`${this.apiUrl}/${this.editTaskId}`, this.newTask);
        const index = this.tasks.findIndex(t => t.id === this.editTaskId);
        if (index !== -1) {
          this.tasks.splice(index, 1, this.newTask);
        }
        this.cancelEdit();
      } catch (err) {
        console.error('Error updating task:', err);
        alert('Failed to update task. Please try again.');
      }
    },
    
    cancelEdit() {
      this.editMode = false;
      this.editTaskId = null;
      this.resetForm();
    },
    
    async toggleTaskStatus(task) {
      try {
        await axios.put(`${this.apiUrl}/${task.id}`, task);
      } catch (err) {
        console.error('Error updating task status:', err);
        alert('Failed to update task status. Please try again.');
        // Revert the change in UI
        task.completed = !task.completed;
      }
    },
    
    async deleteTask(id) {
      if (confirm('Are you sure you want to delete this task?')) {
        try {
          await axios.delete(`${this.apiUrl}/${id}`);
          this.tasks = this.tasks.filter(task => task.id !== id);
        } catch (err) {
          console.error('Error deleting task:', err);
          alert('Failed to delete task. Please try again.');
        }
      }
    },
    
    resetForm() {
      this.newTask = {
        title: '',
        description: '',
        completed: false
      };
    }
  }
};
</script>

<style scoped>
.card {
  transition: transform 0.3s;
}
.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0,0,0,0.1);
}
</style> 