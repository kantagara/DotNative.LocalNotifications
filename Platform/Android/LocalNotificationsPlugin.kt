package com.dotnative.plugins

import android.app.Activity
import android.app.NotificationChannel
import android.app.NotificationManager
import android.os.Build

class LocalNotificationsPlugin(private val activity: Activity) {
    init {
        val channel = NativeChannels.channel(LocalNotificationsChannel)
        channel.handle("show") { args, reply ->
            val fields = args as? Map<*, *>
            val id = (fields?.get("id") as? Long)?.toInt()
            val title = fields?.get("title") as? String
            val body = fields?.get("body") as? String
            if (id == null || title.isNullOrBlank() || body.isNullOrBlank()) {
                reply.failure("invalid_argument", "A notification id, title and body are required")
                return@handle
            }
            try {
                val manager = activity.getSystemService(NotificationManager::class.java)
                if (Build.VERSION.SDK_INT >= 26) manager.createNotificationChannel(NotificationChannel("dotnative.default", "Notifications", NotificationManager.IMPORTANCE_DEFAULT))
                val notification = if (Build.VERSION.SDK_INT >= 26) android.app.Notification.Builder(activity, "dotnative.default") else @Suppress("DEPRECATION") android.app.Notification.Builder(activity)
                notification.setSmallIcon(android.R.drawable.ic_dialog_info).setContentTitle(title).setContentText(body).setAutoCancel(true)
                manager.notify(id, notification.build())
                reply.success()
            } catch (error: SecurityException) { reply.failure("permission_denied", error.message ?: "Notification permission is required") }
            catch (error: Exception) { reply.failure("notification_failed", error.message ?: "Could not show notification") }
        }
    }
}
