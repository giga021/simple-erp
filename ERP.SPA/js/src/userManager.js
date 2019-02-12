import { createUserManager } from 'redux-oidc';

const userManagerConfig = {
  client_id: 'js',
  redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/callback`,
  response_type: 'token id_token',
  scope: 'openid profile knjizenje',
  authority: process.env.REACT_APP_IDENTITY_HOST,
  silent_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/silent-renew`,
  automaticSilentRenew: true,
  filterProtocolClaims: true,
  loadUserInfo: true,
  post_logout_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/`,
};

const userManager = createUserManager(userManagerConfig);
export default userManager;