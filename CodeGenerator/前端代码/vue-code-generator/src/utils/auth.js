import Cookies from 'js-cookie'

const TokenKey = 'Admin-Token'

export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token) {
  return Cookies.set(TokenKey, token)
}

export function setKey(key, value) {
  return Cookies.set(key, value)
}

export function getKey(key) {
  return Cookies.get(key)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}

export function removeKey(key) {
  return Cookies.remove(key)
}
