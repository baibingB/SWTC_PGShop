<template>
	<view class="grace-padding">
		<view style="margin-top:50px;" class="grace-center">
			<image src='../../static/logo.png' style='width:68px; height:68px; border-radius:8px;'></image>
		</view>
		<view class="grace-form" style="margin-top:50upx;">
			<form>
				<view class="grace-space-between  item-border" style="margin-top:28upx;">
					<view class="grace-items" style="width:100%;">
						<view class="grace-label grace-center">账户</view>
						<input v-model="userid" class="input" placeholder="请输入井通地址"></input>
					</view>
				</view>
				<view class="grace-space-between  item-border" style="margin-top:28upx;">
					<view class="grace-items" style="width:100%;">
						<view class="grace-label grace-center">密匙</view>
						<input v-model="userkey" password="true" class="input" placeholder="请输入井通密匙"></input>
					</view>
				</view>
				<button form-type='submit' type='primary' style='background:#00C777; margin-top:30px;' @tap="btnlogin">
					登录</text>
				</button>
			</form>
		</view>
		<view class="grace-center" style="margin-top:20upx; line-height:50upx;">
			要激活钱包，需要转入至少25个SWT
		</view>
	</view>
</template>
<script>
	var _self;
	var graceRandom = require("../../graceUI/jsTools/random.js");
	export default {
		data() {
			return {
				userid: '',
				userkey: ''
			}
		},
		onLoad: function() {
			_self = this;
			var vuserid = uni.getStorageSync('userid');
			var vuserkey = uni.getStorageSync('userkey');
			if (vuserid.length > 0) {
				this.userid = vuserid;
				this.userkey = vuserkey;
			}
		},
		methods: {
			btnlogin: function(e) {
				if (this.userid.length < 1 || this.userkey.length < 1) {
					uni.showToast({
						icon: 'none',
						title: '请输入井通账户和密匙'
					})
				} else {
					var rm = graceRandom.uuid(32);
					uni.request({
						url: 'https://api.jingtum.com/v2/accounts/' + _self.userid + '/payments',
						method: 'POST',
						data: {
							"secret": _self.userkey,
							"client_id": rm,
							"payment": {
								"source": _self.userid,
								"destination": _self.sysUser,
								"amount": {
									"value": "0.000001",
									"currency": "SWT",
									"issuer": ""
								},
								"choice": "",
								"memos": ["LOGIN", "LOGIN"]
							},
						},
						success: function(res) {
							let list = res.data;
							if (list.success == false) {
								uni.showToast({
									icon: 'none',
									title: '井通地址密匙错误或账户未激活'
								});
								return;
							} else {
								uni.setStorageSync('userid', _self.userid);
								uni.setStorageSync('userkey', _self.userkey);
								uni.navigateBack({})
							}
						},
						fail: () => {
							uni.showModal({
								title: '网络提示',
								content: '网络连接错误',
								showCancel: false
							});
							return;
						}
					})
				}
			}
		}
	}
</script>
<style>
	.item-border {
		border-bottom: 1px solid #E0E0E0 !important;
	}

	.login-sendmsg-btn {
		border: 1px solid #00C777 !important;
		background: none !important;
		color: #00C777 !important;
		width: 100%;
		height: 70upx;
		line-height: 70upx;
		margin-top: 6px;
		font-size: 30upx !important;
	}

	.grace-login-three {
		display: flex;
		justify-content: center;
		flex-wrap: nowrap;
	}

	.grace-login-three view {
		width: 50px;
		height: 50px;
		line-height: 50px;
		font-size: 46px;
		color: #00C777;
		text-align: center;
		margin: 8px 15px;
	}
</style>
