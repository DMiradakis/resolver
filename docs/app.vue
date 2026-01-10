<script setup lang="ts">
const runtimeConfig = useRuntimeConfig();

useHead(() => ({
  link: [
    // Manually add canonical reference.
    {
      rel: 'canonical',
      href: () => `${runtimeConfig.public.siteDomain}` + route.path
    }
  ],
  htmlAttrs: {
    lang: 'en',
  }
}))
</script>