<template>
	<view class="grace-body">
		<graceImmersedStatusbar bgColor="#F5F6F7"></graceImmersedStatusbar>
		<view class="grace-line-title">
			<view class="line"></view>
			<view class="title grace-h4">结算</view>
			<view class="line"></view>
		</view>
		<!-- 购物车主结构  -->
		<view class="grace-list grace-shoppingcard">
			<view class='items' @tap="selectAddress">
				<view class="body">
					<view class="title">{{shopaddress.name}}<text>{{shopaddress.tel}}</text></view>
					<view class="desc">{{shopaddress.address}}</view>
				</view>
				<view class="arrow-right"></view>
			</view>
		</view>
		<view class="grace-shoppingcard grace-margin-top" v-for="(item, index) in shoppingCard" :key="index">
			<view class="grace-title grace-flex-vcenter">
				<view>{{item.shopName}}</view>
			</view>
			<view class="goods" v-for="(goods, indexItem) in item.items" :key="indexItem">
				<image :src="goods.img" mode="widthFix"></image>
				<view class="body">
					<view class="goods-title">{{goods.goodsName}}</view>
					<view class="goods-price">
						PG {{goods.price}}
						<view class="goods-number">
							{{goods.count}}
						</view>
					</view>
				</view>
			</view>
		</view>
		<view style="height:120rpx;"></view>
		<!-- 底部  -->
		<view class="grace-footer grace-nowrap grace-flex-vcenter" v-if="shoppingCard.length >= 1">
			<view class="grace-shoppingcard-text">
				合计 : <text style="font-size:36upx; color:#F00;">PG {{totalprice}}</text>
			</view>
			<button type="warn" style="background:#E55D52; width:260rpx; flex-shrink:0;" @tap="checkout">确认订单</button>
		</view>
	</view>
</template>
<script>
	var _self;
	import graceNumberBox from "../../graceUI/components/graceNumberBox.vue";
	import graceEmpty from "../../graceUI/components/graceEmpty.vue";
	import graceCheckBtn from '../../graceUI/components/graceCheckBtn.vue';
	import graceImmersedStatusbar from "../../graceUI/components/graceImmersedStatusbar.vue";
	export default {
		data() {
			return {
				// 总价
				totalprice: '',
				selectText: '全不选',
				// 购物车数据 可以来自 api 请求或本地数据
				goodsShoplist: [],
				shoppingCard: [],
				shopaddress: {}
			}
		},
		onLoad: function(options) {
			_self = this;
			var data = JSON.parse(options.index); // 字符串转对象
			_self.shoppingCard = data;
			_self.countTotoal();
			console.log('1');
		},
		onShow: function() {
			//获取地址
			console.log('2');
			var fuserid = uni.getStorageSync('userid');
			var faddress = uni.getStorageSync('faddress');
			if (faddress.length < 1) {
				uni.showToast({
					icon: 'none',
					title: '收货地址未添加，请在[我的]中添加'
				});
				return;
			}
			_self.gRequest.get(
				_self.webUrlUser + '?method=shopaddress', {
					fuserid: fuserid,
					faddress: faddress
				},
				function(res) {
					console.log(res);
					if (res.res == '1') {
						let data = JSON.parse(res.msg);
						_self.shopaddress = {
							name: data[0].FUSERNAME,
							tel: data[0].FTEL,
							address: data[0].FADDRESS,
							seqno: data[0].FADDRESS
						};
						uni.setStorageSync('faddress', data[0].FSEQNO);
					}
					else{
						_self.shopaddress = {
							name: '',
							tel: '',
							address:'',
							seqno: ''
						};
						uni.setStorageSync('faddress', '');
					}
				}
			);
		},
		methods: {
			//计算总计函数
			countTotoal: function() {
				var total = 0;
				for (var i = 0; i < this.shoppingCard.length; i++) {
					for (var ii = 0; ii < this.shoppingCard[i].items.length; ii++) {
						if (this.shoppingCard[i].items[ii].checked) {
							total += Number(this.shoppingCard[i].items[ii].price) * Number(this.shoppingCard[i].items[ii].count);
						}
					}
				}
				this.totalprice = total;
			},
			numberChange: function(data) {
				this.shoppingCard[data[2]].items[data[1]].count = data[0];
				var fuserid = uni.getStorageSync('userid');
				_self.gRequest.get(
					_self.webUrlUser + '?method=goodsShopChange', {
						fgdseq: _self.shoppingCard[data[2]].items[data[1]].goodsId,
						fuser: fuserid,
						fsl: data[0],
					},
					function(res) {}
				);
				//计算总价
				this.countTotoal();
			},
			removeGoods: function(e) {
				var index = e.currentTarget.id.replace('removeIndex_', '');
				index = index.split('_');
				var index1 = Number(index[0]);
				var index2 = Number(index[1]);
				var fgdseq = index[2];
				var fuserid = uni.getStorageSync('userid');
				wx.showModal({
					title: '确认提醒',
					content: '您确定要移除此商品吗？',
					success: function(e) {
						if (e.confirm) {
							console.log(fgdseq);
							_self.gRequest.get(
								_self.webUrlUser + '?method=goodsShopDel', {
									fgdseq: fgdseq,
									fuser: fuserid
								},
								function(res) {
									console.log(res);
									_self.shoppingCard[index1].items.splice(index2, 1);
									// 是否全部删除包含商铺
									if (_self.shoppingCard[index1].items.length < 1) {
										_self.shoppingCard.splice(index1, 1);
									}
									//计算总价
									_self.countTotoal();
								}
							);
						}
					}
				})
			},
			checkout: function() {
				var fadrdess = uni.getStorageSync('faddress');
				console.log(fadrdess);
				if (fadrdess.length < 1 || fadrdess == 'undefined') {
					uni.showToast({
						icon: 'none',
						title: '收货地址未添加，请在[我的]中添加'
					});
					return;
				}
				console.log(_self.shoppingCard);
				var fgdseq = '';
				var fsl = 0;
				for (let i = 0; i < _self.shoppingCard[0].items.length; i++) {
					fgdseq = fgdseq + ',' + _self.shoppingCard[0].items[i].goodsId;
					fsl = fsl + _self.shoppingCard[0].items[i].count * _self.shoppingCard[0].items[i].price;
				}
				//扣款
				console.log(fsl);
				_self.gRequest.post(
					_self.webUrlSys + '/exchange/payment', {
						secret: uni.getStorageSync('userkey'),
						to: _self.sysUser,
						amount: fsl,
						currency: 'JPG',
						issuer: 'jGa9J9TkqtBcUoHe2zqhVFFbgUVED6o9or'
					}, 'form', {},
					function(res) {
						console.log(res);
						if (res.code == "0") {
							//生成订单
							_self.gRequest.get(
								_self.webUrlUser + '?method=goodsorder', {
									fgdseq: fgdseq,
									fuserid: uni.getStorageSync('userid'),
									faddress: uni.getStorageSync('faddress')
								},
								function(res) {
									uni.navigateBack({})
								}
							);
						} else {
							uni.showToast({
								icon: 'none',
								title: res.msg
							});
						}
					}
				);
			},
			// 店铺选中按钮状态切换
			shopChange: function(e) {
				var index = Number(e[1]);
				this.shoppingCard[index].checked = e[0];
				for (let i = 0; i < this.shoppingCard[index].items.length; i++) {
					this.shoppingCard[index].items[i].checked = e[0];
				}
				this.shoppingCard.splice(index, this.shoppingCard[index]);
				this.countTotoal();
			},
			// 商品选中
			itemChange: function(e) {
				var indexs = e[1].toString();
				var index = indexs.split('-');
				index[0] = Number(index[0]);
				index[1] = Number(index[1]);
				this.shoppingCard[index[0]].items[index[1]].checked = e[0];
				this.shoppingCard.splice(index[0], this.shoppingCard[index[0]]);
				var checkedNum = 0;
				for (let i = 0; i < this.shoppingCard[index[0]].items.length; i++) {
					if (this.shoppingCard[index[0]].items[i].checked) {
						checkedNum++;
					}
				}
				if (checkedNum < 1) {
					this.shoppingCard[index[0]].checked = false;
				} else {
					this.shoppingCard[index[0]].checked = true;
				}
				this.shoppingCard = this.shoppingCard;
				this.countTotoal();
			},
			selectAddress: function() {
				uni.navigateTo({
					url: '../addresslist/addresslist'
				})
			}
		},
		components: {
			graceNumberBox,
			graceEmpty,
			graceCheckBtn,
			graceImmersedStatusbar
		}
	}
</script>
<style>
	page {
		background: #F5F6F7;
	}

	.grace-shoppingcard-checkbtn {
		width: 180rpx;
		margin-left: 50rpx;
		flex-shrink: 0;
	}

	.grace-shoppingcard-check-text {
		font-size: 28rpx;
		margin-left: 12rpx;
		color: #666666;
	}

	.grace-shoppingcard-text {
		width: 700rpx;
		line-height: 100rpx;
	}

	.grace-line-title>.line {
		width: 50rpx;
		flex: auto;
		height: 1px;
		background: #D1D1D1;
		margin: 0 50rpx;
	}

	/* #ifdef H5 */
	.grace-footer {
		bottom: 50px;
	}

	/* #endif */
</style>
