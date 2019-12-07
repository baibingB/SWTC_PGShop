import Vue from 'vue'
import App from './App'
/* 全局引入 graceUI request 工具 */
import graceUIRequest from './graceUI/jsTools/request.js'
Vue.prototype.gRequest = graceUIRequest;
Vue.prototype.webUrlUser = '';
Vue.prototype.webUrlSys = '';
Vue.prototype.webUrlSysInf = '';
Vue.prototype.sysUser = '';
Vue.prototype.sysUserChange = '';
Vue.prototype.sysUserChangePwd = '';
Vue.config.productionTip = false;

/* 封装 setData 函数 */
Vue.prototype.setData   = function(data, thisObj){
	for(let k in data){
		thisObj[k] = data[k];
	}
}

App.mpType = 'app'
const app = new Vue({
    ...App
})
app.$mount()
