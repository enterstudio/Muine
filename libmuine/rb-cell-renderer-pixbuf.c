/* rbcellrendererpixbuf.c
 *
 * arch-tag: Implementation of Rhythmbox pixbuf GtkTreeView cell renderer
 *
 * Copyright (C) 2000  Red Hat, Inc.,  Jonathan Blandford <jrb@redhat.com>
 * Copyright (C) 2002  Jorn Baayen <jorn@nl.linux.org>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Library General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

#include <config.h>
#include <stdlib.h>
#include <stdio.h>

#include "rb-cell-renderer-pixbuf.h"
#include "macros.h"

static void rb_cell_renderer_pixbuf_get_property  (GObject                    *object,
						   guint                       param_id,
						   GValue                     *value,
						   GParamSpec                 *pspec);
static void rb_cell_renderer_pixbuf_set_property  (GObject                    *object,
						   guint                       param_id,
						   const GValue               *value,
						   GParamSpec                 *pspec);
static void rb_cell_renderer_pixbuf_init       (RBCellRendererPixbuf      *celltext);
static void rb_cell_renderer_pixbuf_class_init (RBCellRendererPixbufClass *class);
static void rb_cell_renderer_pixbuf_get_size   (GtkCellRenderer            *cell,
						GtkWidget                  *widget,
						GdkRectangle               *rectangle,
						gint                       *x_offset,
						gint                       *y_offset,
						gint                       *width,
						gint                       *height);
static void rb_cell_renderer_pixbuf_render     (GtkCellRenderer            *cell,
						GdkWindow                  *window,
						GtkWidget                  *widget,
						GdkRectangle               *background_area,
						GdkRectangle               *cell_area,
						GdkRectangle               *expose_area,
						guint                       flags);


enum {
	PROP_ZERO,
	PROP_PIXBUF
};


GType
rb_cell_renderer_pixbuf_get_type (void)
{
	static GType cell_pixbuf_type = 0;

	if (!cell_pixbuf_type)
	{
		static const GTypeInfo cell_pixbuf_info =
		{
			sizeof (RBCellRendererPixbufClass),
			NULL,		/* base_init */
			NULL,		/* base_finalize */
			(GClassInitFunc) rb_cell_renderer_pixbuf_class_init,
			NULL,		/* class_finalize */
			NULL,		/* class_data */
			sizeof (RBCellRendererPixbuf),
			0,              /* n_preallocs */
			(GInstanceInitFunc) rb_cell_renderer_pixbuf_init,
			NULL
		};

		cell_pixbuf_type = g_type_register_static (GTK_TYPE_CELL_RENDERER, "RBCellRendererPixbuf", &cell_pixbuf_info, 0);
	}

	return cell_pixbuf_type;
}

static void
rb_cell_renderer_pixbuf_init (RBCellRendererPixbuf *UNUSED(cellpixbuf))
{
}

static void
rb_cell_renderer_pixbuf_class_init (RBCellRendererPixbufClass *class)
{
	GObjectClass *object_class = G_OBJECT_CLASS (class);
	GtkCellRendererClass *cell_class = GTK_CELL_RENDERER_CLASS (class);

	object_class->get_property = rb_cell_renderer_pixbuf_get_property;
	object_class->set_property = rb_cell_renderer_pixbuf_set_property;

	cell_class->get_size = rb_cell_renderer_pixbuf_get_size;
	cell_class->render = rb_cell_renderer_pixbuf_render;

	g_object_class_install_property (object_class,
					 PROP_PIXBUF,
					 g_param_spec_object ("pixbuf",
							      "Pixbuf Object",
							      "The pixbuf to render.",
							      GDK_TYPE_PIXBUF,
							      G_PARAM_READABLE |
							      G_PARAM_WRITABLE));
}

static void
rb_cell_renderer_pixbuf_get_property (GObject        *object,
				      guint           param_id,
				      GValue         *value,
				      GParamSpec     *pspec)
{
  RBCellRendererPixbuf *cellpixbuf = RB_CELL_RENDERER_PIXBUF (object);
  
  switch (param_id)
    {
    case PROP_PIXBUF:
      g_value_set_object (value,
                          cellpixbuf->pixbuf ? G_OBJECT (cellpixbuf->pixbuf) : NULL);
      break;
    default:
      G_OBJECT_WARN_INVALID_PROPERTY_ID (object, param_id, pspec);
      break;
    }
}


static void
rb_cell_renderer_pixbuf_set_property (GObject      *object,
				      guint         param_id,
				      const GValue *value,
				      GParamSpec   *pspec)
{
  GdkPixbuf *pixbuf;
  RBCellRendererPixbuf *cellpixbuf = RB_CELL_RENDERER_PIXBUF (object);
  
  switch (param_id)
    {
    case PROP_PIXBUF:
      pixbuf = (GdkPixbuf*) g_value_get_object (value);
      if (pixbuf)
        g_object_ref (G_OBJECT (pixbuf));
      if (cellpixbuf->pixbuf)
	g_object_unref (G_OBJECT (cellpixbuf->pixbuf));
      cellpixbuf->pixbuf = pixbuf;
      break;
    default:
      G_OBJECT_WARN_INVALID_PROPERTY_ID (object, param_id, pspec);
      break;
    }
}

/**
 * rb_cell_renderer_pixbuf_new:
 * 
 * Creates a new #RBCellRendererPixbuf. Adjust rendering
 * parameters using object properties. Object properties can be set
 * globally (with g_object_set()). Also, with #RBTreeViewColumn, you
 * can bind a property to a value in a #RBTreeModel. For example, you
 * can bind the "pixbuf" property on the cell renderer to a pixbuf value
 * in the model, thus rendering a different image in each row of the
 * #RBTreeView.
 * 
 * Return value: the new cell renderer
 **/
GtkCellRenderer *
rb_cell_renderer_pixbuf_new (void)
{
  return GTK_CELL_RENDERER (g_object_new (rb_cell_renderer_pixbuf_get_type (), NULL));
}

static GdkPixbuf *
eel_create_colorized_pixbuf (GdkPixbuf *src,
    			     int red_value,
			     int green_value,
			     int blue_value)
{
	int i, j;
	int width, height, has_alpha, src_row_stride, dst_row_stride;
	guchar *target_pixels;
	guchar *original_pixels;
	guchar *pixsrc;
	guchar *pixdest;
	GdkPixbuf *dest;

	g_return_val_if_fail (gdk_pixbuf_get_colorspace (src) == GDK_COLORSPACE_RGB, NULL);
	g_return_val_if_fail ((!gdk_pixbuf_get_has_alpha (src)
		       	      && gdk_pixbuf_get_n_channels (src) == 3)
			      || (gdk_pixbuf_get_has_alpha (src)
			      && gdk_pixbuf_get_n_channels (src) == 4), NULL);
	g_return_val_if_fail (gdk_pixbuf_get_bits_per_sample (src) == 8, NULL);

	dest = gdk_pixbuf_new (gdk_pixbuf_get_colorspace (src),
			       gdk_pixbuf_get_has_alpha (src),
			       gdk_pixbuf_get_bits_per_sample (src),
			       gdk_pixbuf_get_width (src),
			       gdk_pixbuf_get_height (src));

	has_alpha = gdk_pixbuf_get_has_alpha (src);
	width = gdk_pixbuf_get_width (src);
	height = gdk_pixbuf_get_height (src);
	src_row_stride = gdk_pixbuf_get_rowstride (src);
	dst_row_stride = gdk_pixbuf_get_rowstride (dest);
	target_pixels = gdk_pixbuf_get_pixels (dest);
	original_pixels = gdk_pixbuf_get_pixels (src);

	for (i = 0; i < height; i++) {
		pixdest = target_pixels + i*dst_row_stride;
		pixsrc = original_pixels + i*src_row_stride;
		for (j = 0; j < width; j++) {		
			*pixdest++ = *pixsrc++ ? red_value : 0;
			*pixdest++ = *pixsrc++ ? green_value : 0;
			*pixdest++ = *pixsrc++ ? blue_value : 0;
			if (has_alpha) {
				*pixdest++ = *pixsrc++;
			}	
		}
	}
	return dest;
}

static void
rb_cell_renderer_pixbuf_get_size (GtkCellRenderer *cell,
				  GtkWidget       *UNUSED(widget),
				  GdkRectangle    *cell_area,
				  gint            *x_offset,
				  gint            *y_offset,
				  gint            *width,
				  gint            *height)
{
  RBCellRendererPixbuf *cellpixbuf = (RBCellRendererPixbuf *) cell;
  gint pixbuf_width = 0;
  gint pixbuf_height = 0;
  gint calc_width;
  gint calc_height;

  if (cellpixbuf->pixbuf)
    {
      pixbuf_width = gdk_pixbuf_get_width (cellpixbuf->pixbuf);
      pixbuf_height = gdk_pixbuf_get_height (cellpixbuf->pixbuf);
    }
  
  calc_width = (gint) GTK_CELL_RENDERER (cellpixbuf)->xpad * 2 + pixbuf_width;
  calc_height = (gint) GTK_CELL_RENDERER (cellpixbuf)->ypad * 2 + pixbuf_height;
  
  if (x_offset) *x_offset = 0;
  if (y_offset) *y_offset = 0;

  if (cell_area && pixbuf_width > 0 && pixbuf_height > 0)
    {
      if (x_offset)
	{
	  *x_offset = GTK_CELL_RENDERER (cellpixbuf)->xalign * (cell_area->width - calc_width - (2 * GTK_CELL_RENDERER (cellpixbuf)->xpad));
	  *x_offset = MAX (*x_offset, 0) + GTK_CELL_RENDERER (cellpixbuf)->xpad;
	}
      if (y_offset)
	{
	  *y_offset = GTK_CELL_RENDERER (cellpixbuf)->yalign * (cell_area->height - calc_height - (2 * GTK_CELL_RENDERER (cellpixbuf)->ypad));
	  *y_offset = MAX (*y_offset, 0) + GTK_CELL_RENDERER (cellpixbuf)->ypad;
	}
    }

  if (calc_width)
    *width = calc_width;
  
  if (height)
    *height = calc_height;
}

static void
rb_cell_renderer_pixbuf_render (GtkCellRenderer    *cell,
				GdkWindow          *window,
				GtkWidget          *widget,
				GdkRectangle       *UNUSED(background_area),
				GdkRectangle       *cell_area,
				GdkRectangle       *UNUSED(expose_area),
				guint               flags)

{
  RBCellRendererPixbuf *cellpixbuf = (RBCellRendererPixbuf *) cell;
  GdkPixbuf *pixbuf;
  GdkRectangle pix_rect;
  GdkRectangle draw_rect;
  GtkStateType state;

  if ((flags & GTK_CELL_RENDERER_SELECTED) == GTK_CELL_RENDERER_SELECTED)
    {
      if (GTK_WIDGET_HAS_FOCUS (widget))
        state = GTK_STATE_SELECTED;
      else
        state = GTK_STATE_ACTIVE;
    }
  else
    {
      if (GTK_WIDGET_STATE (widget) == GTK_STATE_INSENSITIVE)
        state = GTK_STATE_INSENSITIVE;
      else
        state = GTK_STATE_NORMAL;
    }

  if (!cellpixbuf->pixbuf)
    return;

  pixbuf = eel_create_colorized_pixbuf (cellpixbuf->pixbuf,
					widget->style->text[state].red,
					widget->style->text[state].green,
					widget->style->text[state].blue);

  if (!pixbuf)
    return;

  rb_cell_renderer_pixbuf_get_size (cell, widget, cell_area,
				     &pix_rect.x,
				     &pix_rect.y,
				     &pix_rect.width,
				     &pix_rect.height);
  
  pix_rect.x += cell_area->x;
  pix_rect.y += cell_area->y;
  pix_rect.width -= cell->xpad * 2;
  pix_rect.height -= cell->ypad * 2;
  
  if (gdk_rectangle_intersect (cell_area, &pix_rect, &draw_rect))
    gdk_draw_pixbuf (window,
		     widget->style->black_gc,
                     pixbuf,
                     /* pixbuf 0, 0 is at pix_rect.x, pix_rect.y */
                     draw_rect.x - pix_rect.x,
                     draw_rect.y - pix_rect.y,
                     draw_rect.x,
                     draw_rect.y,
                     draw_rect.width,
                     draw_rect.height,
		     GDK_RGB_DITHER_NORMAL,
		     0, 0);

  g_object_unref (pixbuf);
}
