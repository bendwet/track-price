import { CognitoAuth } from "amazon-cognito-auth-js";

const authData = {
    ClientId: '',
    AppWebDomain: '',
    TokenScopesArray: ['openid', 'email', 'profile'],
    RedirectUriSignIn: 'http://localhost:3000/',
    RedirectUriSignOut: 'http://localhost:3000/'
  }
  
const auth = new CognitoAuth(authData)
export default auth;