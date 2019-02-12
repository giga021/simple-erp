import 'whatwg-fetch';
import { parseJson, showApiError } from 'utils'
import store from "store";

const KontaApi = {
  getKonta: () => {
    return fetch(`/api/konta`, {
      method: 'GET',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    })
      .then(parseJson)
      .catch(e => showApiError(e.message))
  }
}

export default KontaApi;