<template>
	<view class="grace-body">
		<view class="grace-list">
			<view class='items' v-for="(item, index) in forData" :key="index">
				<view class="body">
					<view class="title">{{item.name}}<text>{{item.tel}}</text></view>
					<view class="desc">{{item.ads}}</view>
				</view>
				<view v-bind:id="item.seqno" class="arrow-right" @tap="selectAddress"></view>
			</view>
		</view>
	</view>
</template>
<script>
	var _self;
	export default {
		data() {
			return {
				forData: [] //模拟的循环数据
			};
		},
		onLoad: function() {
			_self = this;
			//检查是否有未扫描完成
			uni.request({
				url: _self.webUrlUser + '?method=addres&str1=' + uni.getStorageSync('userid'),
				success: function(res) {
					let list = res.data;
					if (list.res == "0") {
						return;
					}
					let datalist = JSON.parse(list.msg)
					for (let i = 0; i < datalist.length; i++) {
						let item = {
							name: datalist[i].FUSERNAME,
							ads: datalist[i].FADDRESS,
							tel: datalist[i].FTEL,
							area: datalist[i].FAREA,
							seqno: datalist[i].FSEQNO
						};
						_self.forData.push(item);
					}
				},
				fail: () => {
					uni.showModal({
						title: '网络提示',
						content: '网络连接错误',
						showCancel: false
					});
				}
			})
		},
		methods: {
			selectAddress: function(e) {
				uni.setStorageSync('faddress', e.target.id);
				uni.navigateBack({})
			}
		}
	}
</script>
<style>
</style>