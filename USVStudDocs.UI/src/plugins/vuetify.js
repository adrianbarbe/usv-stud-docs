// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Vuetify
import { createVuetify } from 'vuetify'


const usvTheme = {
    dark: false,
    colors: {
        background: '#FFFFFF',
        surface: '#FFFFFF',
        primary: "#264796",
        // 'primary-darken-1': '#264796',
        secondary: '#03DAC6',
        // 'secondary-darken-1': '#018786',
        error: '#B00020',
        info: '#2196F3',
        success: '#4CAF50',
        warning: '#FB8C00',
    }
}

export default createVuetify(
    {
        theme: {
            defaultTheme: 'usvTheme',
            themes: {
                usvTheme,
            }
        }
    }
)
