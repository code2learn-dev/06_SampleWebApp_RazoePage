jalaliDatepicker.startWatch({
	minDate: "attr",
	maxDate: "attr",
	time: true
});

let addForm = document.getElementById('add-form');
let userSelectedMovieId = 0;

setTimeout(async () => {
	await getMoviesList();
}, 2000);

async function getMoviesList() {
	const response = await fetch('https://localhost:7081/movies/MoviesJsonResultList');
	if (!response.ok) {
		console.log(`server error ${response.status}-${response.text()}`);
		return;
	}

	const movies = await response.json();
	if (!Array.isArray(movies) || movies.length === 1) {
		const alertMessage = document.createElement('p');
		alertMessage.className = 'alert alert-warning text-center';
		alertMessage.innerText = 'فیلمی یافت نشد';
		addForm.appendChild(alertMessage);
		return;
	}

	const table = document.createElement('table');
	table.className = "table table-striped table-bordered table-responsive";
	table.setAttribute('id', 'table-result');
	table.innerHTML =
		'<thead>' +
		'<tr>' +
		'<th>ردیف</th>' +
		'<th>عنوان</th>' +
		'<th>امتیاز</th>' +
		'<th>انتخاب فیلم</th>' +
		'</tr>' +
		'</thead>';
	const tbody = document.createElement('tbody');

	movies.forEach((m, index) => {
		const movieRow = document.createElement('tr');
		movieRow.innerHTML = `
			<td>${index + 1}</td>
			<td>${m.title}</td>
			<td>${m.score}</td>
			<td>
				<a class="btn btn-success btn-sm select-movie" data-movie-id="${m.id}">
					انتخاب فیلم
				</a>
			</td>
		`;
		tbody.appendChild(movieRow);
	});

	table.appendChild(tbody);

	$('div.loading-container').fadeOut(() => {
		addForm.innerHTML = '';
		setTimeout(async () => {
			addForm.appendChild(table);
			$('table#table-result').fadeIn();
			await selectMovie();
			await highlightSelectedMovie();
		}, 100);
	});
}

function selectMovie() {
	const selectMovieLinkList = document.querySelectorAll('a.select-movie');
	selectMovieLinkList.forEach(link => {
		link.addEventListener('click', (e) => {
			const movieId = e.target.dataset.movieId;
			setTimeout(async () => {
				const response = await fetch(`https://localhost:7081/movies/storemovie/${movieId}`);

				if (!response.ok) {
					window.location.href = 'https://localhost:7081/tickets/index';
					return;
				}

				link.style.pointerEvents = 'none';
				link.style.backgroundColor = '#353e35';
				link.style.color = '#fff';
				deActivateLinks(e.target.dataset.movieId);
				await callCustomersList(movieId);
			});
		});
	});
}

function deActivateLinks(activeId) {
	const linkList = document.querySelectorAll('a.select-movie');
	linkList.forEach(link => {
		if (link.dataset.movieId != activeId) {
			link.style.pointerEvents = 'auto';
			link.removeAttribute('style');
		}
		if (link.dataset.movieId == activeId) {
			link.style.pointerEvents = 'none';
			link.style.backgroundColor = '#353e35';
			link.style.color = '#fff';
		}
	})
}

async function highlightSelectedMovie(movieId) {
	if (movieId === null || movieId === 0) return;

	const readStoredMovieResponse = await fetch(`https://localhost:7081/movies/readstoredmovie/${movieId}`);
	if (!readStoredMovieResponse.ok) {
		userSelectedMovieId = 0;
		console.log(`server error: ${response.text()}`);
		retturn;
	}

	const selectedMovie = await readStoredMovieResponse.json();
	if (selectedMovie !== null) {
		userSelectedMovieId = selectedMovie['id'];
		deActivateLinks(userSelectedMovieId);
		console.log(userSelectedMovieId);
	}


	const nextBtnContainer = document.createElement('div');
	nextBtnContainer.className = 'text-start';
	const nextStepBtn = document.createElement('a');
	nextStepBtn.className = 'btn btn-success';
	nextStepBtn.setAttribute('id', 'next-step-btn');
	nextStepBtn.innerText = 'مرحله بعدی'
	nextStepBtn.addEventListener('click', async () => {
		await callCustomersList(movieId);
	});

	nextBtnContainer.appendChild(nextStepBtn);

	addForm.appendChild(nextBtnContainer);
	$('#next-step-btn').fadeIn();
}

async function callCustomersList(movieId) {
	if (movieId === null || movieId == 0) {
		console.log('servert error: Movie Id is null');
		return;
	}

	$('#table-result').fadeOut();
	$('#next-step-btn').fadeOut();
	$('div.loading-container').fadeIn();

	setTimeout(async () => {
		const movie = await displaySelectedMovie(movieId);
		await getCustomersInTable(movie['id']);
	}, 1100);

	setTimeout(() => {
		selectCustomer();
	}, 2500);
}

async function displaySelectedMovie(movieId) {
	const response = await fetch(`https://localhost:7081/movies/readstoredmovie/${movieId}`);
	if (!response.ok) {
		alert('فیلمی جهت رزور یافت نشد');
		return;
	}

	const movie = await response.json();

	$('table#table-result').fadeOut(() => {
		const movieContainer = document.createElement('div');
		movieContainer.className = 'selected-movie';

		const movieImg = document.createElement('img');
		movieImg.setAttribute('src', `https://localhost:7249/images/movies/${movie['imageName']}`);
		movieImg.setAttribute('alt', movie['title']);

		movieContainer.appendChild(movieImg);

		const movieCaptionDiv = document.createElement('div');
		movieCaptionDiv.className = 'movie-caption';

		const movieTitle = document.createElement('p');
		movieTitle.innerHTML = `<i class="bi bi-film"></i> عنوان فیلم انتخاب شده ${movie['title']}`;
		movieTitle.className = 'alert alert-info p-2';

		movieCaptionDiv.appendChild(movieTitle);

		const movieScore = document.createElement('p');
		movieScore.innerHTML = `<i class="bi bi-star-half"></i> امتیاز فیلم ${movie['score']}`;
		movieScore.className = 'alert alert-warning p-2';

		movieCaptionDiv.appendChild(movieScore);

		const movieBtnContainer = document.createElement('div');
		movieBtnContainer.className = 'text-start mt-2';

		const movieBackBtn = document.createElement('p');
		movieBackBtn.className = 'btn btn-warning';
		movieBackBtn.innerHTML = `<i class="bi bi-arrow-right-circle"></i> انتخاب فیلم دیگر`;
		movieBackBtn.addEventListener('click', () => {
			$('div.loading-container').fadeIn();
			setTimeout(async () => {
				await getMoviesList();
			});
		});

		movieBtnContainer.appendChild(movieBackBtn);
		movieCaptionDiv.appendChild(movieBtnContainer);

		movieContainer.appendChild(movieCaptionDiv);

		addForm.appendChild(movieContainer);
		document.getElementById('table-result').style.display = 'none';
	});

	return movie;
}

async function getCustomersInTable(movieId) {
	const response = await fetch('https://localhost:7081/Customer/CustomerListAsJson');
	if (!response.ok) {
		//location.reload();
		window.location.href = 'https://localhost:7081/Tickets/Index';
		return;
	}

	const customers = await response.json();
	if (!Array.isArray(customers) || customers.length === 0) {
		const alertMessage = document.createElement('p');
		alertMessage.className = 'alert alert-warning';
		alertMessage.innerText = 'مشتری یافت نشد';
		addForm.appendChild(alertMessage);
		return;
	}

	const table = document.createElement('table');
	table.className = "table table-striped table-bordered table-responsive";
	table.setAttribute('id', 'customer-table-result');
	table.innerHTML =
		'<thead>' +
		'<tr>' +
		'<th>ردیف</th>' +
		'<th>نام مشتری</th>' +
		'<th>کد ملی</th>' +
		'<th>انتخاب مشتری</th>' +
		'</tr>' +
		'</thead>';
	const tbody = document.createElement('tbody');

	customers.forEach((m, index) => {
		const dataRow = document.createElement('tr');
		dataRow.innerHTML = `
			<td>${index + 1}</td>
			<td>${m.firstName} ${m.lastName}</td>
			<td>${m.nationalCode}</td>
			<td>
				<a class="btn btn-success btn-sm select-customer" data-customer-id="${m.id}" data-movie-id="${movieId}">
					انتخاب مشتری
				</a>
			</td>
		`;
		tbody.appendChild(dataRow);
	});

	table.appendChild(tbody);

	setTimeout(() => {
		$('div.loading-container').fadeOut();
		addForm.appendChild(table);
		$('#customer-table-result').fadeIn();
		selectCustomer();
	}, 2000);

}

function selectCustomer() {
	const selectCustomerBtnGroup = document.querySelectorAll('a.select-customer');
	selectCustomerBtnGroup.forEach(c => {
		c.addEventListener('click', (e) => {
			setTimeout(async () => {
				$('div.loading-container').fadeIn();
				await createTicketForCustomer(e.target.dataset.customerId, e.target.dataset.movieId);
			})
		});
	});
}

async function createTicketForCustomer(customerId, movieId) {
	if (movieId === null || movieId == 0) {
		alert('فیلمی جهت رزور یافت نشد');
		return;
	}

	const response = await fetch(`https://localhost:7081/Customer/GetCustomerAsJson/${customerId}`);
	if (!response.ok) {
		window.location.href = 'https://localhost:7081/Tickets/Index';
		return;
	}

	const customerProjectionModel = await response.json();
	if (customerProjectionModel === null) {
		window.location.href = 'https://localhost:7081/Tickets/Index';
		return;
	}

	const registerFormResponse = await fetch(`https://localhost:7081/tickets/register/${customerProjectionModel['id']}`);

	if (!registerFormResponse.ok) {
		console.log('مشتری یافت نشد');
		return;
	}

	$('#customer-table-result').fadeOut(500, () => {
		registerFormResponse.text()
			.then(html => {
				const registerForm = document.getElementById('register-form');
				if (registerForm) {
					registerForm.remove();
				}

				setTimeout(() => {
					addForm.insertAdjacentHTML('beforeend', html);
					$('#register-form').slideDown(700);

					// Re-parse validation attributes in the new form
					$.validator.unobtrusive.parse('#register-form');

					$('div.loading-container').fadeOut();

					document.getElementById('change-customer-btn').addEventListener('click',
						async () => {
							$('div.loading-container').fadeIn();
							document.getElementById('register-form').remove();
							await getCustomersInTable(movieId);
						});
				}, 1500);
			});
	});

}