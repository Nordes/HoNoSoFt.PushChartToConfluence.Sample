<template>
  <div class="page-title">
    <h1>{{ title }}</h1>
  </div>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      default: 'Empty title'
    }
  }
}
</script>
