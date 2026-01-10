export default defineNuxtConfig({
    modules: ['@nuxt/ui', 'nuxt-gtag'],
    nitro: {
        preset: 'static'
    },
    site: {
        name: 'Resolver',
    },
    gtag: {
        id: 'G-QLFG8NVHYX'
    }
})