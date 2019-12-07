<template>
	<view class="grace-body">
		<grace-speaker :vertical="false" :interval="5000" iconClass="grace-icons icon-gonggao" :msgs="speakerMsgs"></grace-speaker>
		<view class="grace-list">
			<view class="items">
				<view class="checkBtns">
					<graceCheckBtn :checked="typecheck==='SWT'" @change="checkedChange1" :size="46"></graceCheckBtn>
				</view>
				<view class="body">
					<view class="title">SWTC</view>
				</view>
				<view class="checkBtns">
					<graceCheckBtn :checked="typecheck==='JMOAC'" @change="checkedChange2" :size="46"></graceCheckBtn>
				</view>
				<view class="body">
					<view class="title">MOAC</view>
				</view>
				<view class="checkBtns">
					<graceCheckBtn :checked="typecheck==='JJCC'" @change="checkedChange3" :size="46"></graceCheckBtn>
				</view>
				<view class="body">
					<view class="title">JCC</view>
				</view>
				<view class="checkBtns">
					<graceCheckBtn :checked="typecheck==='CSP'" @change="checkedChange4" :size="46"></graceCheckBtn>
				</view>
				<view class="body">
					<view class="title">CSP</view>
				</view>
			</view>
		</view>
		<form @submit="formSubmit" class="grace-form grace-margin-top">
			<view class="grace-items">
				<view class="grace-label">兑换PG数量</view>
				<view class="other" @tap="showKeyboard">{{numberVal == '' ? '点击这里输入兑换的PG数' : numberVal}}</view>
			</view>
			<view class="grace-items">
				<view class="grace-label">兑换比例(自动)</view>
				<view class="other">≈{{numberBl}}</view>
			</view>
		</form>
		<!-- 数字键盘组件 -->
		<graceNumberKeyboard :show="graceNumberKeyboardShow" v-on:keyboardDone="keyboardDone" v-on:keyboardInput="keyboardInput"
		 v-on:keyboardDelete="keyboardDelete">
		</graceNumberKeyboard>
		<button type="primary" class="grace-button" style="margin-top: 50px;" @tap="btnSure">确认兑换</button>
	</view>
</template>
<script>
	var _self;
	import graceCheckBtn from '../../graceUI/components/graceCheckBtn.vue';
	import graceNumberKeyboard from '../../graceUI/components/graceNumberKeyboard.vue';
	import graceSpeaker from "../../graceUI/components/graceSpeaker.vue";
	var graceRandom = require("../../graceUI/jsTools/random.js");
	export default {
		data() {
			return {
				typecheck: 'SWT',
				token: ' ',
				graceNumberKeyboardShow: false,
				numberVal: '10',
				numberBl:'',
				speakerMsgs: [{
						title: "PG官方兑换，已认证"
					},
					{
						title: "请根据需要兑换的情况填写"
					},
					{
						title: "如有疑问，请联系客服"
					}
				]
			}
		},
		onLoad: function(options) {
			_self = this;
		},
		onShow:function(){
			_self.bljs();
		},
		methods: {
			bljs:function(){
			},
			checkedChange1: function(e) {
				_self.typecheck = 'SWT';
				_self.token = ' ';
				_self.bljs();
			},
			checkedChange2: function(e) {
				_self.typecheck = 'JMOAC';
				_self.token = 'jGa9J9TkqtBcUoHe2zqhVFFbgUVED6o9or';
				_self.bljs();
			},
			checkedChange3: function(e) {
				_self.typecheck = 'JJCC';
				_self.token = 'jGa9J9TkqtBcUoHe2zqhVFFbgUVED6o9or';
				_self.bljs();
			},
			checkedChange4: function(e) {
				_self.typecheck = 'CSP';
				_self.token = 'jGa9J9TkqtBcUoHe2zqhVFFbgUVED6o9or';
				_self.bljs();
			},
			showKeyboard: function() {
				//打开数字键盘
				this.graceNumberKeyboardShow = true;
			},
			// 监听输入
			keyboardInput: function(e) {
				this.numberVal = this.numberVal + '' + e;
				_self.bljs();
			},
			// 监听删除
			keyboardDelete: function() {
				this.numberVal = this.numberVal.substring(0, this.numberVal.length - 1);
				_self.bljs();
			},
			// 完成事件
			keyboardDone: function() {
				this.graceNumberKeyboardShow = false;
				_self.bljs();
			},
			btnSure: function() {
			}
		},
		components: {
			graceCheckBtn,
			graceNumberKeyboard,
			"grace-speaker": graceSpeaker
		}
	}
</script>
<style>
	.checkBtns {
		width: 50rpx;
	}
</style>
