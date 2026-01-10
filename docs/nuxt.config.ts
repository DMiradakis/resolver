export default defineNuxtConfig({
    modules: [
      '@nuxt/ui',
      'nuxt-gtag',
      '@nuxtjs/sitemap'
    ],
    nitro: {
        preset: 'static'
    },
    site: { 
        url: 'https://resolver.solopreneur.sh', 
        name: 'Resolver' 
    }, 
    gtag: {
        id: 'G-QLFG8NVHYX'
    },
    robots: {
        sitemap: '/sitemap.xml'
    },
})