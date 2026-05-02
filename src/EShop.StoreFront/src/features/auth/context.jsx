import { createContext, useMemo, useContext } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getProfile } from "./api";
import { API_BASE_URL } from "@env";

const AuthContext = createContext(null);

const returnUrl = encodeURIComponent(
  window.location.pathname + window.location.search,
);

const getProfileSafe = async () => {
  try {
    const { data } = await getProfile();
    return data ?? null;
  } catch (error) {
    if (error?.response?.status === 401 || error?.status === 401) {
      /*
       * If the user is not authenticated, we can return null for the profile.
       * For case guest user, we can return null for the profile as well.
       */
      return null;
    }
    throw error;
  }
};

export const AuthProvider = ({ children }) => {
  const queryClient = useQueryClient();

  const {
    data: user,
    isLoading,
    isFetching,
    error,
    refetch,
  } = useQuery({
    queryKey: ["auth", "profile"],
    queryFn: getProfileSafe,
    staleTime: 5 * 60 * 1000,
    retry: false,
  });

  const login = async () => {
    const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
    window.location.href = `${API_BASE_URL}/login?returnUrl=${returnUrl}`;
  };

  const logout = async () => {
    queryClient.setQueryData(["auth", "profile"], null);
    const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
    window.location.href = `${API_BASE_URL}/logout?returnUrl=${returnUrl}`;
  };

  const ctx = useMemo(
    () => ({
      user,
      isAuthenticated: Boolean(user),
      isLoading,
      isFetching,
      error,
      login,
      logout,
      refetchProfile: refetch,
    }),
    [user, isLoading, isFetching, error, refetch],
  );

  return <AuthContext.Provider value={ctx}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error("useAuth must be used within a AuthProvider");
  }
  return ctx;
};
