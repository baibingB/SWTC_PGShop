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
			console.log(e);
			// 获取传递过来的链接
			this.url = e.furl;
		}
	}
</script>
