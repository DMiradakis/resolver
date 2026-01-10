const siteName = 'Resolver';
const siteDomain = 'https://resolver.solopreneur.sh'

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
        url: siteDomain, 
        name: siteName 
    }, 
    gtag: {
        id: 'G-QLFG8NVHYX'
    },
    robots: {
        sitemap: '/sitemap.xml'
    },
    runtimeConfig: {
        public: {
            siteDomain: siteDomain,
            siteName: siteName,
            twitterSite: '@dmiradev',
            twitterSiteUrl: 'https://x.com/dmiradev',
            youtubeProfileUrl: 'https://www.youtube.com/@SolopreneurDev',
        }
    },
})