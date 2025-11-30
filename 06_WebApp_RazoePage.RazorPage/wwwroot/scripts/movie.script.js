$(document).ready(function () {
	let image = $("#movie-image");
	let btnSelectImage = $("#select-image");
	let inputFile = $("#input-file");

	image.fadeIn(700);

	btnSelectImage.click(function () {
		inputFile.click();
	});

	$("#clear-image").click(function () {
		let currentImage = $("#current-image").val();
		if (currentImage)
			image.attr('src', currentImage);
		else
			image.fadeOut();

		inputFile.reset();
	})

	inputFile.change(function () {
		if (this.files) {
			let file = this.files[0];

			let reader = new FileReader();
			reader.onload = function (event) {
				image.attr('src', event.target.result);
			}

			reader.readAsDataURL(file);
			image.fadeIn(700);
		}
	});
});

let inputTagName = document.getElementById('input-tag');
let suggestionTagList = document.querySelector('.suggestion-tags-list');
let selectedTagsList = document.querySelector('div.selected-tags-list');
let selectedTagsArray = [];
readTagsFromCookie(); 

suggestionTagNames();

//submitTagsList();

//console.clear();
//selectedTagsArray.forEach(a => console.log(a));

function suggestionTagNames() {
	inputTagName.addEventListener('input', (event) => {
		const query = event.target.value;

		if (query.length <= 2) {
			suggestionTagList.style.bottom = '-2.5rem';
			suggestionTagList.style.opacity = '0.0';
			suggestionTagList.innerHTML = '';
			return
		}

		setTimeout(async () => {
			try {
				const response = await fetch(`https://localhost:7081/Tags/Search/${encodeURIComponent(query)}`);
				if (!response.ok) {
					console.log(`server error: ${response.status}`);
					return;
				}

				const tags = await response.json();
				if (!Array.isArray(tags) || tags.length === 0) {
					suggestionTagList.style.bottom = '-2.5rem';
					suggestionTagList.style.opacity = '0.0';
					return;
				}

				let tagsElements = tags.map(tag => { return `<p class="alert alert-warning p-1 m-1" data-tag-id=${tag.id}>${tag.name}</p>` }).join('');
				suggestionTagList.innerHTML = tagsElements;
				suggestionTagList.style.bottom = '-.5rem';
				suggestionTagList.style.opacity = '1.0';

				var searchResultItems = document.querySelectorAll('div.suggestion-tags-list p');

				if (searchResultItems !== null) {
					searchResultItems.forEach(p => {
						p.addEventListener('click', () => {
							const indexTagToAdd = selectedTagsArray.findIndex(tag => tag.id == p.dataset.tagId);
							if (indexTagToAdd === -1) {
								const tagId = p.dataset.tagId;
								createHtmlTagList(p.textContent, tagId);
							}
						});
					});
				}

				//console.log(tagsElements);
			} catch (err) {
				console.log(`server error: ${err}`);
				suggestionTagList.innerHTML = '';
			}
		}, 400);
	})
}

function readTagsFromCookie() { 
	const decodedCookie = document.cookie.split(';').map(a => a.trim());
	const tagCookie = decodedCookie.find(c => c.startsWith('tags='));
	if (!tagCookie) return [];

	const resultValue = decodeURIComponent(tagCookie.split('=')[1]);
	try { 
		const tagsList = JSON.parse(resultValue); 
		tagsList.forEach(tag => {  
			createHtmlTagList(tag.name, tag.id);
		});
		console.log(selectedTagsArray);
	} catch {
		return [];
	}
} 

function createHtmlTagList(tagText, tagId) {
	let newSelectedTag = document.createElement('p');
	let iconTag = document.createElement('i');

	newSelectedTag.className = 'alert alert-warning ml-1';
	newSelectedTag.setAttribute('data-tag-name', tagText);
	newSelectedTag.setAttribute('data-tag-id', tagId);
	iconTag.className = 'bi bi-x-lg';
	iconTag.addEventListener('click', () => {
		let tagName = newSelectedTag.dataset.tagName;
		let tagToRemoveId = newSelectedTag.dataset.tagId;

		const indexToRemove = selectedTagsArray.findIndex(tag => tag.id == tagToRemoveId);
		console.log(tagToRemoveId, indexToRemove);
		if (indexToRemove != -1) {
			selectedTagsArray.splice(indexToRemove, 1);
		}

		newSelectedTag.style.opacity = '0.0';
		newSelectedTag.style.bottom = '-2rem';
		setTimeout(() => {
			if (newSelectedTag.parentElement === selectedTagsList) {
				selectedTagsList.removeChild(newSelectedTag);
			}
		}, 300);

		setCookieValue();
	});
	 
	newSelectedTag.append(iconTag, ' ', tagText);

	selectedTagsList.appendChild(newSelectedTag);
	selectedTagsList.style.opacity = '1.0';
	selectedTagsList.style.bottom = 0;
	setTimeout(() => {
		newSelectedTag.style.opacity = '1.0';
		newSelectedTag.style.bottom = 0;
	}, 100);

	selectedTagsArray.push({ name: tagText, id: tagId });

	suggestionTagList.style.bottom = '-2.5rem';
	suggestionTagList.style.opacity = '0.0';
	suggestionTagList.innerHTML = '';
	inputTagName.value = '';

	setCookieValue();
}

function setCookieValue() {
	const value = encodeURIComponent(JSON.stringify(selectedTagsArray));
	let date = new Date();
	date.setDate(date.getDate() + (1 * 24 * 60 * 60 * 1000));
	let expire = `expires=${date.toUTCString()}`;
	document.cookie = `tags=${value}; expire=${expire}; path=/;`;
}

function submitTagsList() {
	document.querySelector('form').addEventListener('submit', (event) => {
		event.preventDefault();

		//if (this.valid && selectedTagsArray)
		const serialiedTags = JSON.stringify(selectedTagsArray);
		document.getElementById('input-tags-list').value = serialiedTags;

		//this.submit();
	})
}