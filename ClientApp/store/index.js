import Vue from 'vue'
import Vuex from 'vuex'
import I18n from './modules/i18n'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    i18n: I18n
  }
})
