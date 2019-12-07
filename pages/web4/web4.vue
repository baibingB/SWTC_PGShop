<template>
	<view>
		<web-view :webview-styles="webviewStyles" :src="url"></web-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				url: '',
				webviewStyles: {
					progress: {
						color: '#000'
					}
				}
			}
		},
		onLoad(e) {
			this.url = 'https://app.jccdex.cn/#/home/market/exchange/cnt';
		}
	}
</script>
