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
			this.url = 'https://www.swtcwallet.net/#/inviteRegister?uid=d00e373cc4664cdfbea1b78d794706fa';
		}
	}
</script>
