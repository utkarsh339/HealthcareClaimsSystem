export function getErrorMessage(error, fallback = "Something went wrong. Please try again.") {
  if (!error) return fallback;

  if (typeof error === "string") return error;

  const data = error.data;

  if (typeof data === "string" && data.trim()) return data;

  if (data?.message) return data.message;

  if (Array.isArray(data?.errors)) {
    const messages = data.errors.filter(Boolean);
    if (messages.length) return messages.join(" ");
  }

  if (data?.errors && typeof data.errors === "object") {
    const messages = Object.values(data.errors)
      .flat()
      .filter(Boolean)
      .map(String);
    if (messages.length) return messages.join(" ");
  }

  if (data?.title) return data.title;
  if (data?.detail) return data.detail;
  if (error.error) return error.error;
  if (error.status === "FETCH_ERROR") return "Unable to connect to the server. Please check that the API is running.";
  if (error.status === "TIMEOUT_ERROR") return "The request timed out. Please try again.";

  return fallback;
}
