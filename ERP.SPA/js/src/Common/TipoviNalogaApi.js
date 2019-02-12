import 'whatwg-fetch';
import { parseJson } from '../utils'
import store from "store";

const TipoviNalogaApi = {
  getTipovi: () => {
    return fetch(`/api/tipoviNaloga`, {
      method: 'GET',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    }).then(parseJson);
  }
}

export default TipoviNalogaApi;