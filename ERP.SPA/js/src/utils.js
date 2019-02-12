import showMessage from './showMessage'

const parseJson = (response) => {
  if (response.ok) {
    return response.json();
  }
  else {
    let error = `${response.status} ${response.statusText}`;
    showApiError(error, response.url);
  }
}

const showApiError = (message, url) => {
  showMessage.error(message, 'API greška', url);
}

export { parseJson, showApiError };