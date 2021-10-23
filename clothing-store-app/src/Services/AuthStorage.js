const AuthenticationPropNames = ["role", "user", "token"]

const localStorageToken = "credentials"

export default class AuthStorage {
  static validate(credentials, message) {
    if (!AuthenticationPropNames.every(key => credentials.hasOwnProperty(key) && credentials[key])) {
      throw new Error(message ? message : "Invalid credentials in localStorage")
    }
  }

  static load() {
    const credentials = JSON.parse(localStorage.getItem(localStorageToken))
    try {
      this.validate(credentials)
      return credentials
    } catch (e) {
      return
    }
  }

  static save(credentials) {
    this.validate(credentials, "Invalid credentials provided")
    localStorage.setItem(localStorageToken, JSON.stringify(credentials))
  }

  static remove(credentials) {
    this.validate(credentials, "Invalid credentials provided")
    const localStorageCredentials = this.load()

    if (JSON.stringify(credentials) === JSON.stringify(localStorageCredentials)) {
      localStorage.removeItem(localStorageToken)
    }
  }
}
