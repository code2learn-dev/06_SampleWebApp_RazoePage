let image = document.getElementById('customer-image');
let selectImageBtn = document.getElementById('select-image');
let inputFile = document.getElementById('input-file');

selectImageBtn.addEventListener('click', () => {
	inputFile.click();
});
inputFile.addEventListener('change', () => { 
	if (inputFile.files) {
		let file = inputFile.files[0];
		let reader = new FileReader();
		reader.onload = function (event) {
			image.setAttribute('src', event.target.result);
		}

		reader.readAsDataURL(file);
		$('#customer-image').fadeIn();
	}
});

document.getElementById('clear-image').addEventListener('click', () => { 
	$('#customer-image').fadeOut();
	inputFile.value = '';
})