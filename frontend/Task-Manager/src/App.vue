<script setup>
import TaskList from './components/TaskList.vue'
import 'bootstrap/dist/css/bootstrap.min.css'
</script>

<template>
  <div class="app">
    <nav class="navbar navbar-dark bg-primary">
      <div class="container">
        <span class="navbar-brand mb-0 h1">Task Manager</span>
      </div>
    </nav>
    <TaskList />
  </div>
</template>

<style>
.app {
  min-height: 100vh;
  background-color: #f8f9fa;
}
</style>
