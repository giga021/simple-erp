import 'whatwg-fetch';
import { parseJson } from 'utils'
import store from "store";

const KarticaKontaApi = {

  getKarticaKonta: (id) => {
    return fetch(`/api/karticaKonta/${id}`, {
      method: 'GET',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    }).then(parseJson)
  },
}

export default KarticaKontaApi;