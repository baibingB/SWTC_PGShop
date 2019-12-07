<template>
	<view>
		<graceImmersedStatusbar bgColor="#FFFFFF"></graceImmersedStatusbar>
		<view class="grace-body grace-margin-top">
			<view class="grace-h4 grace-margin-top" style="padding:20rpx 0;">个人中心</view>
			<view class="grace-list" style="margin-top:10rpx; margin-bottom:30rpx;">
				<navigator class="items" @tap="nofunc()">
					<view class="icons ucenter-face">
						<image src="https://graceui.oss-cn-beijing.aliyuncs.com/faces/1.png" mode="widthFix"></image>
					</view>
					<view class="body">
						<view class="title">PG官方研发</view>
						<view class="desc">描述信息</view>
					</view>
					<view class="arrow-right"></view>
				</navigator>
			</view>
		</view>
		<view class="ucenter-line"></view>
		<view class="grace-list grace-body">
			<navigator class="items" @tap="nofunc">
				<view class="icons grace-icons icon-friend grace-blue"></view>
				<view class="body">
					<view class="title">好友动态</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
			<navigator class="items" url="../changePg/changePg">
				<view class="icons grace-icons icon-pwd grace-green"></view>
				<view class="body">
					<view class="title">兑换PG</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
			<navigator class="items" url="../qrcode/qrcode">
				<view class="icons grace-icons icon-friend grace-green"></view>
				<view class="body">
					<view class="title">付款二维码</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
			<navigator class="items" url="../money/money">
				<view class="icons grace-icons icon-wallet grace-green"></view>
				<view class="body">
					<view class="title">我的钱包</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
			<navigator class="items" url="../myorders/myorders">
				<view class="icons grace-icons icon-order grace-red"></view>
				<view class="body">
					<view class="title">我的订单</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
			<navigator class="items">
				<view class="icons grace-icons icon-user grace-red"></view>
				<view class="body">
					<view class="title">App分享</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
		</view>
		<view class="ucenter-line"></view>
		<view class="grace-list grace-body">
			<navigator class="items" @tap="ToAddress()">
				<view class="icons grace-icons icon-set grace-yellow"></view>
				<view class="body">
					<view class="title">收货地址</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
			<navigator class="items" url="../login/login">
				<view class="icons grace-icons icon-article grace-blue-sky"></view>
				<view class="body">
					<view class="title">账户设置</view>
				</view>
				<view class="arrow-right"></view>
			</navigator>
		</view>
		<view class="ucenter-line"></view>
	</view>
</template>
<script>
	import graceImmersedStatusbar from "../../graceUI/components/graceImmersedStatusbar.vue";
	export default {
		components: {
			graceImmersedStatusbar
		},
		methods: {
			ToAddress: function() {
				var vuserid = uni.getStorageSync('userid');
				if (vuserid.length > 0) {
					uni.navigateTo({
						url: '../address/address'
					})
				} else {
					uni.navigateTo({
						url: '../login/login'
					})
				}
			},
			nofunc:function(){
				uni.showToast({
					title:'更多功能敬请期待'
				});
			}
		}
	}
</script>
<style>
	.ucenter-face {
		width: 120rpx !important;
		height: 120rpx !important;
	}

	.ucenter-face image {
		width: 120rpx !important;
		height: 120rpx !important;
	}

	.ucenter-line {
		width: 100%;
		height: 10px;
		background: #F4F5F6;
		margin: 8px 0;
	}
</style>
